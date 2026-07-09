using Ariana_Mcp.integrations.Exceptions;
using Ariana_Mcp.integrations.Helpers;

namespace Ariana_Mcp.integrations.Services;

public sealed class OrderService(IHttpClientFactory httpClientFactory)
    : ArianaLabServiceBase(httpClientFactory)
{
    private const int DefaultSearchLimit = 25;
    private const int MaxSearchLimit = 50;

    public async Task<string> SearchOrdersAsync(
        string? q,
        int limit = DefaultSearchLimit,
        CancellationToken cancellationToken = default)
    {
        limit = Math.Clamp(limit, 1, MaxSearchLimit);
        var query = string.IsNullOrWhiteSpace(q)
            ? EasyQueryBuilder.BuildJson(limit: limit)
            : EasyQueryBuilder.EnsureLimit(q, limit);
        var body = await GetQueryAsync("Rest/Opd/Probenanlage/Auftraege", query, cancellationToken);
        return HalResponseHelper.EnsureEmbeddedOrReturnRaw(body);
    }

    public async Task<string> GetOrderAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArianaLabException("id darf nicht leer sein.");

        return await GetAsync($"Rest/Opd/Probenanlage/Auftraege/{Uri.EscapeDataString(id)}", cancellationToken);
    }

    public async Task<string> SearchCustomerOrdersAsync(
        string? q,
        int limit = DefaultSearchLimit,
        CancellationToken cancellationToken = default)
    {
        limit = Math.Clamp(limit, 1, MaxSearchLimit);
        var query = string.IsNullOrWhiteSpace(q)
            ? EasyQueryBuilder.BuildJson(limit: limit)
            : EasyQueryBuilder.EnsureLimit(q, limit);
        var body = await GetQueryAsync("Rest/Opd/Probenanlage/Kundenauftraege", query, cancellationToken);
        return HalResponseHelper.EnsureEmbeddedOrReturnRaw(body);
    }

    public async Task<string> GetCustomerOrderAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArianaLabException("id darf nicht leer sein.");

        return await GetAsync(
            $"Rest/Opd/Probenanlage/Kundenauftraege/{Uri.EscapeDataString(id)}",
            cancellationToken);
    }

    public async Task<string> GetPlanningOrdersAsync(
        string module,
        string? q,
        int limit = DefaultSearchLimit,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(module))
            throw new ArianaLabException("module darf nicht leer sein.");

        return module.ToLowerInvariant() switch
        {
            "auftraege" or "probenanlage" or "intern" => await SearchOrdersAsync(q, limit, cancellationToken),
            "kundenauftraege" or "kundenauftrag" => await SearchCustomerOrdersAsync(q, limit, cancellationToken),
            _ => throw new ArianaLabException(
                $"Unbekanntes Planungsmodul '{module}'. Erlaubt: auftraege, kundenauftraege."),
        };
    }

    public async Task<string> GetPlanningOrderAsync(
        string module,
        string id,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(module))
            throw new ArianaLabException("module darf nicht leer sein.");

        return module.ToLowerInvariant() switch
        {
            "auftraege" or "probenanlage" or "intern" => await GetOrderAsync(id, cancellationToken),
            "kundenauftraege" or "kundenauftrag" => await GetCustomerOrderAsync(id, cancellationToken),
            _ => throw new ArianaLabException(
                $"Unbekanntes Planungsmodul '{module}'. Erlaubt: auftraege, kundenauftraege."),
        };
    }
}
