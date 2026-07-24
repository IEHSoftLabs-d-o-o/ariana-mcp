using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;

namespace Ariana_Mcp.Okf;

public sealed class OkfService
{
    private const int MaxContentChars = 24_000;
    private const int DefaultSearchLimit = 8;
    private const int MaxSearchLimit = 20;

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true,
        PropertyNamingPolicy = null,
    };

    private static readonly Regex MetadataLineRegex = new(
        @"^([A-Za-z0-9_-]+):\s*(.+?)\s*$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static readonly Regex WhitespaceRegex = new(
        @"\s+",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private readonly string _bundleRoot;
    private readonly object _cacheGate = new();
    private Task<IReadOnlyList<string>>? _markdownPathsTask;
    private Task<IReadOnlyList<OkfDocument>>? _documentsTask;

    public OkfService(IOptions<OkfOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);
        var configured = options.Value.BundlePath?.Trim();
        if (string.IsNullOrEmpty(configured))
            throw new OkfException("OKF bundle path is not configured.");

        _bundleRoot = ResolveBundleRoot(configured);
    }

    public async Task<string> GetBundleStatusAsync(CancellationToken cancellationToken = default)
    {
        EnsureBundleDirectory();
        var paths = await GetMarkdownPathsAsync(cancellationToken).ConfigureAwait(false);
        var rootIndex = await ReadMarkdownAsync("index.md", cancellationToken).ConfigureAwait(false);
        return JsonSerializer.Serialize(
            new
            {
                markdownDocuments = paths.Count,
                rootIndex,
            },
            JsonOptions);
    }

    public Task<string> ReadIndexAsync(string? path, CancellationToken cancellationToken = default)
    {
        var requested = path?.Trim() ?? string.Empty;
        string relative;
        if (string.IsNullOrEmpty(requested))
            relative = "index.md";
        else if (requested.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
            relative = requested;
        else
            relative = $"{requested.TrimEnd('/')}/index.md";

        return ReadMarkdownAsync(relative, cancellationToken);
    }

    public Task<string> ReadConceptAsync(string path, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new OkfException("path is required");

        return ReadMarkdownAsync(path.Trim(), cancellationToken);
    }

    public async Task<string> SearchAsync(
        string query,
        int? limit = null,
        CancellationToken cancellationToken = default)
    {
        var normalized = query?.Trim().ToLowerInvariant() ?? string.Empty;
        if (string.IsNullOrEmpty(normalized))
            throw new OkfException("query is required");

        var terms = normalized
            .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        var resultLimit = limit is null
            ? DefaultSearchLimit
            : Math.Max(1, Math.Min(MaxSearchLimit, limit.Value));

        var documents = await GetDocumentsAsync(cancellationToken).ConfigureAwait(false);
        var results = documents
            .Select(document =>
            {
                var body = document.Content.ToLowerInvariant();
                var pathText = document.RelativePath.ToLowerInvariant();
                var metadata = string.Join(' ', document.Metadata.Values).ToLowerInvariant();
                var score = body.Contains(normalized, StringComparison.Ordinal) ? 20 : 0;
                foreach (var term in terms)
                {
                    score += CountOccurrences(pathText, term) * 12;
                    score += CountOccurrences(metadata, term) * 6;
                    score += Math.Min(CountOccurrences(body, term), 10);
                }

                return (document, score);
            })
            .Where(x => x.score > 0)
            .OrderByDescending(x => x.score)
            .ThenBy(x => x.document.RelativePath, StringComparer.Ordinal)
            .Take(resultLimit)
            .Select(x =>
            {
                var body = WhitespaceRegex.Replace(x.document.Content, " ").Trim();
                var lower = body.ToLowerInvariant();
                var positions = terms
                    .Select(term => lower.IndexOf(term, StringComparison.Ordinal))
                    .Where(p => p >= 0)
                    .ToArray();
                var position = positions.Length > 0 ? positions.Min() : 0;
                var start = Math.Max(0, position - 140);
                var end = Math.Min(body.Length, position + 360);
                var excerpt =
                    $"{(start > 0 ? "…" : "")}{body[start..end]}{(end < body.Length ? "…" : "")}";

                x.document.Metadata.TryGetValue("title", out var title);
                x.document.Metadata.TryGetValue("type", out var type);
                x.document.Metadata.TryGetValue("confidence", out var confidence);

                return new
                {
                    path = x.document.RelativePath,
                    score = x.score,
                    title,
                    type,
                    confidence,
                    excerpt,
                };
            })
            .ToArray();

        return JsonSerializer.Serialize(
            new
            {
                query,
                count = results.Length,
                results,
            },
            JsonOptions);
    }

    private async Task<string> ReadMarkdownAsync(string relativePath, CancellationToken cancellationToken)
    {
        var fullPath = SafeBundlePath(relativePath);
        if (!fullPath.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
            throw new OkfException("Only Markdown can be read");

        string content;
        try
        {
            content = await File.ReadAllTextAsync(fullPath, Encoding.UTF8, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (FileNotFoundException)
        {
            throw new OkfException($"Markdown file not found: {NormalizeRelative(relativePath)}");
        }
        catch (DirectoryNotFoundException)
        {
            throw new OkfException($"Markdown file not found: {NormalizeRelative(relativePath)}");
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new OkfException("Unable to read the requested Markdown file.", ex);
        }
        catch (IOException ex)
        {
            throw new OkfException("Unable to read the requested Markdown file.", ex);
        }

        return content.Length <= MaxContentChars
            ? content
            : $"{content[..MaxContentChars]}\n\n[truncated]";
    }

    private string SafeBundlePath(string relativePath)
    {
        var normalized = NormalizeRelative(relativePath);
        if (Path.IsPathRooted(relativePath) ||
            normalized.Split('/', StringSplitOptions.RemoveEmptyEntries).Any(part => part == ".."))
        {
            throw new OkfException("Path must stay inside the OKF bundle");
        }

        var resolved = Path.GetFullPath(Path.Combine(_bundleRoot, normalized.Replace('/', Path.DirectorySeparatorChar)));
        var rootWithSep = _bundleRoot.EndsWith(Path.DirectorySeparatorChar)
            ? _bundleRoot
            : _bundleRoot + Path.DirectorySeparatorChar;

        if (!string.Equals(resolved, _bundleRoot, StringComparison.OrdinalIgnoreCase) &&
            !resolved.StartsWith(rootWithSep, StringComparison.OrdinalIgnoreCase))
        {
            throw new OkfException("Path must stay inside the OKF bundle");
        }

        return resolved;
    }

    private static string NormalizeRelative(string relativePath) =>
        relativePath.Replace('\\', '/').TrimStart('/');

    private void EnsureBundleDirectory()
    {
        if (!Directory.Exists(_bundleRoot))
            throw new OkfException("Configured OKF bundle path is missing or not a directory.");
    }

    private Task<IReadOnlyList<string>> GetMarkdownPathsAsync(CancellationToken cancellationToken)
    {
        lock (_cacheGate)
        {
            _markdownPathsTask ??= Task.Run(LoadMarkdownPaths);
        }

        cancellationToken.ThrowIfCancellationRequested();
        return _markdownPathsTask;
    }

    private IReadOnlyList<string> LoadMarkdownPaths()
    {
        EnsureBundleDirectory();
        var files = ListMarkdownFiles(_bundleRoot);
        files.Sort(StringComparer.OrdinalIgnoreCase);
        return files;
    }

    private static List<string> ListMarkdownFiles(string directory)
    {
        var results = new List<string>();
        foreach (var entry in Directory.EnumerateFileSystemEntries(directory))
        {
            if (Directory.Exists(entry))
                results.AddRange(ListMarkdownFiles(entry));
            else if (entry.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
                results.Add(entry);
        }

        return results;
    }

    private Task<IReadOnlyList<OkfDocument>> GetDocumentsAsync(CancellationToken cancellationToken)
    {
        lock (_cacheGate)
        {
            _documentsTask ??= LoadDocumentsAsync();
        }

        cancellationToken.ThrowIfCancellationRequested();
        return _documentsTask;
    }

    private async Task<IReadOnlyList<OkfDocument>> LoadDocumentsAsync()
    {
        var files = await GetMarkdownPathsAsync(CancellationToken.None).ConfigureAwait(false);
        var documents = new List<OkfDocument>(files.Count);
        foreach (var file in files)
        {
            var content = await File.ReadAllTextAsync(file, Encoding.UTF8).ConfigureAwait(false);
            var relative = Path.GetRelativePath(_bundleRoot, file).Replace('\\', '/');
            documents.Add(new OkfDocument(relative, content, ParseMetadata(content)));
        }

        return documents;
    }

    private static Dictionary<string, string> ParseMetadata(string content)
    {
        if (!content.StartsWith("---", StringComparison.Ordinal))
            return new Dictionary<string, string>(StringComparer.Ordinal);

        var end = content.IndexOf("\n---", 3, StringComparison.Ordinal);
        if (end < 0)
            return new Dictionary<string, string>(StringComparer.Ordinal);

        var metadata = new Dictionary<string, string>(StringComparer.Ordinal);
        var block = content[3..end];
        foreach (var line in block.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
        {
            var match = MetadataLineRegex.Match(line);
            if (!match.Success)
                continue;

            var value = match.Groups[2].Value.Trim();
            if (value.Length >= 2 &&
                ((value[0] == '"' && value[^1] == '"') || (value[0] == '\'' && value[^1] == '\'')))
            {
                value = value[1..^1];
            }

            metadata[match.Groups[1].Value] = value;
        }

        return metadata;
    }

    private static int CountOccurrences(string text, string term)
    {
        if (string.IsNullOrEmpty(term))
            return 0;

        var total = 0;
        var index = 0;
        while ((index = text.IndexOf(term, index, StringComparison.Ordinal)) >= 0)
        {
            total++;
            index += term.Length;
        }

        return total;
    }

    private static string ResolveBundleRoot(string configured)
    {
        var candidate = Path.IsPathRooted(configured)
            ? configured
            : Path.Combine(AppContext.BaseDirectory, configured);

        string resolved;
        try
        {
            resolved = Path.GetFullPath(candidate);
        }
        catch (Exception ex)
        {
            throw new OkfException("Configured OKF bundle path is invalid.", ex);
        }

        if (!Directory.Exists(resolved))
            throw new OkfException("Configured OKF bundle path is missing or not a directory.");

        return resolved;
    }

    private sealed record OkfDocument(
        string RelativePath,
        string Content,
        IReadOnlyDictionary<string, string> Metadata);
}
