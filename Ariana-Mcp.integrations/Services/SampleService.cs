using System.Net;
using Ariana_Mcp.integrations.Exceptions;

namespace Ariana_Mcp.integrations.Services;

public sealed class SampleService(IHttpClientFactory httpClientFactory)
    : ArianaLabServiceBase(httpClientFactory)
{
    public async Task<string> GetSampleByIdAsync(
        string sampleId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sampleId))
            throw new ArianaLabException("sampleId darf nicht leer sein.");

        var client = CreateClient();
        var requestUri = $"Rest/Opd/Proben/{Uri.EscapeDataString(sampleId)}/";

        try
        {
            return await GetAsStringAsync(client, requestUri, cancellationToken);
        }
        catch (ArianaLabException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new ArianaLabException($"Keine Probe mit der ID '{sampleId}' gefunden.", HttpStatusCode.NotFound, ex);
        }
    }
}
