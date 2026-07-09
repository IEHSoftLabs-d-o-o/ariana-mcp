using Ariana_Mcp.integrations.Helpers;

namespace Ariana_Mcp.integrations.Services;

public sealed class ReferenceDataService(IHttpClientFactory httpClientFactory)
    : ArianaLabServiceBase(httpClientFactory)
{
    public Task<string> GetPublicAnalysesAsync(CancellationToken cancellationToken = default) =>
        GetAsync("Rest/Public/Analysen", cancellationToken);

    public async Task<string> GetAnalysisAsync(string id, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        return await GetAsync($"Rest/Public/Analysen/{Uri.EscapeDataString(id)}", cancellationToken);
    }

    public Task<string> GetAnalysisByIdAsync(string id, CancellationToken cancellationToken = default) =>
        GetAnalysisAsync(id, cancellationToken);

    public async Task<string> SearchAnalysesAsync(
        string? q,
        bool includeTechnical = false,
        int limit = 50,
        CancellationToken cancellationToken = default)
    {
        limit = Math.Clamp(limit, 1, 200);
        var body = await GetPublicAnalysesAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(q))
            return body;

        var items = HalResponseHelper.ExtractEmbeddedItems(body);
        var term = q.Trim();
        var comparison = StringComparison.OrdinalIgnoreCase;

        var matches = items.Where(item =>
        {
            var name = HalResponseHelper.TryGetStringProperty(item, "Name") ?? string.Empty;
            var description = HalResponseHelper.TryGetStringProperty(item, "Beschreibung") ?? string.Empty;
            return name.Contains(term, comparison) || description.Contains(term, comparison);
        }).ToList();

        if (!includeTechnical)
        {
            matches = matches.Where(item =>
                !string.Equals(
                    HalResponseHelper.TryGetStringProperty(item, "Name"),
                    "Technisch",
                    StringComparison.OrdinalIgnoreCase)).ToList();
        }

        matches = matches.Take(limit).ToList();
        return System.Text.Json.JsonSerializer.Serialize(new { search = term, count = matches.Count, items = matches });
    }

    public Task<string> GetMethodsAsync(bool plainList = false, CancellationToken cancellationToken = default)
    {
        var uri = plainList
            ? "Rest/Mad/Beauftragung/Methoden?plainList=true"
            : "Rest/Mad/Beauftragung/Methoden";
        return GetAsync(uri, cancellationToken);
    }

    public Task<string> GetProductClassesAsync(
        string context = "public",
        CancellationToken cancellationToken = default)
    {
        var uri = context.ToLowerInvariant() switch
        {
            "mad" => "Rest/Mad/Produktklassifizierung/Produktklassen",
            "cor" => "Rest/Cors/master-data/product-classes",
            _ => "Rest/Public/Produktklassen",
        };

        return GetAsync(uri, cancellationToken);
    }

    public Task<string> ListLabParametersAsync(
        string? term,
        bool skipNotInUse = true,
        CancellationToken cancellationToken = default)
    {
        var uri = WithQuery(
            "Rest/Mad/Listen/Parameters",
            ("term", term),
            ("skipNotInUse", skipNotInUse.ToString().ToLowerInvariant()));
        return GetAsync(uri, cancellationToken);
    }

    public Task<string> ListUnitsAsync(CancellationToken cancellationToken = default) =>
        GetAsync("Rest/Mad/Listen/Einheiten", cancellationToken);

    public Task<string> ListProductGroupsAsync(
        string? term,
        CancellationToken cancellationToken = default)
    {
        var uri = string.IsNullOrWhiteSpace(term)
            ? "Rest/Mad/Listen/Produktgruppen"
            : $"Rest/Mad/Listen/Produktgruppen?term={Uri.EscapeDataString(term)}";
        return GetAsync(uri, cancellationToken);
    }

    public Task<string> ListSampleGroupsAsync(
        string? term,
        CancellationToken cancellationToken = default)
    {
        var uri = string.IsNullOrWhiteSpace(term)
            ? "Rest/Mad/Listen/Probengruppen"
            : $"Rest/Mad/Listen/Probengruppen?term={Uri.EscapeDataString(term)}";
        return GetAsync(uri, cancellationToken);
    }

    public Task<string> ListTestPackagesAsync(
        string? term,
        bool skipNotInUse = true,
        CancellationToken cancellationToken = default)
    {
        var uri = WithQuery(
            "Rest/Mad/Listen/Pruefpakete",
            ("term", term),
            ("skipNotInUse", skipNotInUse.ToString().ToLowerInvariant()));
        return GetAsync(uri, cancellationToken);
    }
}
