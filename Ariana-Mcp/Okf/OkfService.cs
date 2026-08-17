using System.Security.Cryptography;
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
    private const string ReferencePrefix = "okf-";

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

    private static readonly Regex MarkdownLinkRegex = new(
        @"!?\[([^\]]*)\]\(((?:[^()]|\([^)]*\))*)\)",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static readonly Regex UriSchemeRegex = new(
        @"^[a-z][a-z0-9+.-]*:",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

    private static readonly Regex MarkdownReferenceLinkRegex = new(
        @"!?\[([^\]]+)\]\[[^\]]*\]",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static readonly Regex MarkdownReferenceDefinitionRegex = new(
        @"^\s*\[[^\]]+\]:\s*\S+.*$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline);

    private static readonly Regex HtmlAnchorRegex = new(
        @"<a\b[^>]*>(.*?)</a>",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);

    private static readonly Regex ResourceUriLineRegex = new(
        @"^\s*resource:\s*[""']?[a-z][a-z0-9+.-]*://.*$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Multiline);

    private static readonly Regex AbsoluteUrlRegex = new(
        @"(?<![A-Za-z0-9])(?:(?:https?|ftp)://|www\.)[^\s<>""']+",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

    private static readonly Regex McpUriRegex = new(
        @"(?<![A-Za-z0-9])(?:okf|arianalab)://[^\s<>""']+",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

    private readonly string _bundleRoot;
    private readonly object _cacheGate = new();
    private Task<IReadOnlyList<string>>? _markdownPathsTask;
    private Task<IReadOnlyList<OkfDocument>>? _documentsTask;
    private Task<OkfReferences>? _referencesTask;

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

    public async Task<string> ReadIndexAsync(string? reference, CancellationToken cancellationToken = default)
    {
        var requested = reference?.Trim() ?? string.Empty;
        var relative = string.IsNullOrEmpty(requested)
            ? "index.md"
            : await ResolveReferenceAsync(requested, isIndex: true, cancellationToken).ConfigureAwait(false);

        return await ReadMarkdownAsync(relative, cancellationToken).ConfigureAwait(false);
    }

    public async Task<string> ReadConceptAsync(string reference, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(reference))
            throw new OkfException("reference is required");

        var relative = await ResolveReferenceAsync(reference.Trim(), isIndex: false, cancellationToken)
            .ConfigureAwait(false);

        return await ReadMarkdownAsync(relative, cancellationToken).ConfigureAwait(false);
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
        var references = await GetReferencesAsync(cancellationToken).ConfigureAwait(false);
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
                var body = WhitespaceRegex
                    .Replace(StripLinkSyntax(RemoveFrontMatter(x.document.Content)), " ")
                    .Trim();
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
                    reference = references.GetReference(x.document.RelativePath),
                    score = x.score,
                    title = title is null ? null : StripLinkSyntax(title),
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

        var references = await GetReferencesAsync(cancellationToken).ConfigureAwait(false);
        var sanitized = Sanitize(content, NormalizeRelative(relativePath), references);
        return sanitized.Length <= MaxContentChars
            ? sanitized
            : $"{sanitized[..MaxContentChars]}\n\n[truncated]";
    }

    /// <summary>
    /// Removes every trace of the underlying storage layout: front matter, links, URLs and
    /// resource URIs. Links that point at bundle documents are replaced by opaque references
    /// so navigation stays possible without exposing file names or paths.
    /// </summary>
    private static string Sanitize(string content, string relativePath, OkfReferences references)
    {
        var sanitized = RemoveFrontMatter(content);
        sanitized = HtmlAnchorRegex.Replace(sanitized, "$1");
        sanitized = MarkdownLinkRegex.Replace(
            sanitized,
            match => ReplaceLink(match, relativePath, references));

        return RemoveLinkArtifacts(sanitized);
    }

    private static string StripLinkSyntax(string content)
    {
        var sanitized = HtmlAnchorRegex.Replace(content, "$1");
        sanitized = MarkdownLinkRegex.Replace(sanitized, "$1");
        return RemoveLinkArtifacts(sanitized);
    }

    private static string RemoveLinkArtifacts(string content)
    {
        var sanitized = MarkdownReferenceLinkRegex.Replace(content, "$1");
        sanitized = MarkdownReferenceDefinitionRegex.Replace(sanitized, string.Empty);
        sanitized = ResourceUriLineRegex.Replace(sanitized, string.Empty);
        sanitized = AbsoluteUrlRegex.Replace(sanitized, string.Empty);
        sanitized = McpUriRegex.Replace(sanitized, string.Empty);
        return sanitized.Replace("<externer Link entfernt>", string.Empty, StringComparison.OrdinalIgnoreCase);
    }

    private static string RemoveFrontMatter(string content)
    {
        if (!content.StartsWith("---", StringComparison.Ordinal))
            return content;

        var closing = content.IndexOf("\n---", 3, StringComparison.Ordinal);
        if (closing < 0)
            return content;

        var lineEnd = content.IndexOf('\n', closing + 1);
        return lineEnd < 0 ? string.Empty : content[(lineEnd + 1)..].TrimStart('\r', '\n');
    }

    private static string ReplaceLink(Match match, string relativePath, OkfReferences references)
    {
        var text = match.Groups[1].Value.Trim();
        if (match.Value.StartsWith('!'))
            return text;

        var reference = ResolveLinkTarget(match.Groups[2].Value, relativePath, references);
        if (reference is null)
            return text;

        return text.Length == 0 ? $"(Referenz: {reference})" : $"{text} (Referenz: {reference})";
    }

    private static string? ResolveLinkTarget(string target, string relativePath, OkfReferences references)
    {
        var candidate = target.Trim();
        if (candidate.StartsWith('<') && candidate.EndsWith('>'))
            candidate = candidate[1..^1].Trim();

        var titleStart = candidate.IndexOfAny([' ', '\t']);
        if (titleStart >= 0)
            candidate = candidate[..titleStart];

        var fragment = candidate.IndexOf('#');
        if (fragment >= 0)
            candidate = candidate[..fragment];

        candidate = candidate.Trim();
        if (candidate.Length == 0 || UriSchemeRegex.IsMatch(candidate))
            return null;

        var combined = candidate.StartsWith('/')
            ? candidate.TrimStart('/')
            : Combine(GetDirectory(relativePath), candidate);

        var normalized = NormalizeSegments(combined);
        if (normalized is null)
            return null;

        if (normalized.Length == 0)
            normalized = "index.md";
        else if (!normalized.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
            normalized = $"{normalized}/index.md";

        return references.TryGetReference(normalized, out var reference) ? reference : null;
    }

    private async Task<string> ResolveReferenceAsync(
        string requested,
        bool isIndex,
        CancellationToken cancellationToken)
    {
        if (requested.StartsWith(ReferencePrefix, StringComparison.OrdinalIgnoreCase))
        {
            var references = await GetReferencesAsync(cancellationToken).ConfigureAwait(false);
            if (references.TryGetPath(requested, out var mapped))
                return mapped;

            throw new OkfException("Unknown reference. Use okf_search or an index to obtain a valid reference.");
        }

        if (requested.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
            return requested;

        return isIndex ? $"{requested.TrimEnd('/')}/index.md" : requested;
    }

    private static string GetDirectory(string relativePath)
    {
        var separator = relativePath.LastIndexOf('/');
        return separator < 0 ? string.Empty : relativePath[..separator];
    }

    private static string Combine(string directory, string candidate) =>
        directory.Length == 0 ? candidate : $"{directory}/{candidate}";

    private static string? NormalizeSegments(string path)
    {
        var segments = new List<string>();
        foreach (var segment in path.Split('/', StringSplitOptions.RemoveEmptyEntries))
        {
            if (segment == ".")
                continue;

            if (segment == "..")
            {
                if (segments.Count == 0)
                    return null;

                segments.RemoveAt(segments.Count - 1);
                continue;
            }

            segments.Add(segment);
        }

        return string.Join('/', segments);
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

    private Task<OkfReferences> GetReferencesAsync(CancellationToken cancellationToken)
    {
        lock (_cacheGate)
        {
            _referencesTask ??= LoadReferencesAsync();
        }

        cancellationToken.ThrowIfCancellationRequested();
        return _referencesTask;
    }

    private async Task<OkfReferences> LoadReferencesAsync()
    {
        var files = await GetMarkdownPathsAsync(CancellationToken.None).ConfigureAwait(false);
        return new OkfReferences(files.Select(ToRelativePath));
    }

    private string ToRelativePath(string fullPath) =>
        Path.GetRelativePath(_bundleRoot, fullPath).Replace('\\', '/');

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
            documents.Add(new OkfDocument(ToRelativePath(file), content, ParseMetadata(content)));
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

    /// <summary>
    /// Stable, opaque handles for bundle documents. Callers never receive file names or paths.
    /// </summary>
    private sealed class OkfReferences
    {
        private readonly Dictionary<string, string> _pathByReference = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> _referenceByPath = new(StringComparer.OrdinalIgnoreCase);

        public OkfReferences(IEnumerable<string> relativePaths)
        {
            foreach (var relativePath in relativePaths)
            {
                var reference = CreateReference(relativePath);
                _pathByReference[reference] = relativePath;
                _referenceByPath[relativePath] = reference;
            }
        }

        public bool TryGetPath(string reference, out string relativePath) =>
            _pathByReference.TryGetValue(reference, out relativePath!);

        public bool TryGetReference(string relativePath, out string reference) =>
            _referenceByPath.TryGetValue(relativePath, out reference!);

        public string GetReference(string relativePath) =>
            _referenceByPath.TryGetValue(relativePath, out var reference)
                ? reference
                : CreateReference(relativePath);

        private static string CreateReference(string relativePath)
        {
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(relativePath.ToLowerInvariant()));
            return ReferencePrefix + Convert.ToHexStringLower(hash.AsSpan(0, 5));
        }
    }
}
