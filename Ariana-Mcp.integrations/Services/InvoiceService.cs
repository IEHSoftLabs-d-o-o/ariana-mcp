using Ariana_Mcp.integrations.Exceptions;
using Ariana_Mcp.integrations.Helpers;

namespace Ariana_Mcp.integrations.Services;

public sealed class InvoiceService(IHttpClientFactory httpClientFactory)
    : ArianaLabServiceBase(httpClientFactory)
{
    public async Task<string> SearchInvoicesAsync(
        string? q,
        CancellationToken cancellationToken = default)
    {
        var body = await GetQueryAsync("Rest/Opd/Rechnungserstellung/Rechnungen", q, cancellationToken);
        return HalResponseHelper.EnsureEmbeddedOrReturnRaw(body);
    }

    public async Task<string> GetInvoiceAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArianaLabException("id darf nicht leer sein.");

        return await GetAsync(
            $"Rest/Opd/Rechnungserstellung/Rechnungen/{Uri.EscapeDataString(id)}",
            cancellationToken);
    }
}
