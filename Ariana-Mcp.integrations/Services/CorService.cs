using Ariana_Mcp.integrations.Exceptions;
using Ariana_Mcp.integrations.Helpers;

namespace Ariana_Mcp.integrations.Services;

public sealed class CorService(IHttpClientFactory httpClientFactory)
    : ArianaLabServiceBase(httpClientFactory)
{
    public async Task<string> SearchCorAsync(
        string? q,
        CancellationToken cancellationToken = default)
    {
        var body = await GetQueryAsync("Rest/Cors/CustomerOrderRequests", q, cancellationToken);
        return HalResponseHelper.EnsureEmbeddedOrReturnRaw(body);
    }

    public async Task<string> GetCorAsync(string corId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(corId))
            throw new ArianaLabException("corId darf nicht leer sein.");

        var encoded = ArianaLabUriHelper.EncodePathSegment(corId);
        return await GetAsync($"Rest/Cors/CustomerOrderRequests/{encoded}", cancellationToken);
    }

    public async Task<string> ValidateCorGatewayAsync(
        string dtoJson,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dtoJson))
            throw new ArianaLabException("dto darf nicht leer sein.");

        object? body;
        try
        {
            body = System.Text.Json.JsonSerializer.Deserialize<object>(dtoJson);
        }
        catch (System.Text.Json.JsonException ex)
        {
            throw new ArianaLabException("dto muss gültiges JSON sein.", innerException: ex);
        }

        return await PostAsStringAsync("Rest/Cors/CustomerOrderRequestsGateway/validate", body, cancellationToken);
    }
}
