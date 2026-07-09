using System.Net;
using Ariana_Mcp.Integrations.AraianLab;
using Ariana_Mcp.integrations.Exceptions;

namespace Ariana_Mcp.integrations.Services;

public abstract class ArianaLabServiceBase(IHttpClientFactory httpClientFactory)
{
    protected HttpClient CreateClient() => httpClientFactory.CreateClient(ArianaLabHttp.ClientName);

    protected static async Task<string> GetAsStringAsync(
        HttpClient client,
        string requestUri,
        CancellationToken cancellationToken)
    {
        using var response = await client.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);
        var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
            return body;

        throw CreateHttpException(response.StatusCode, requestUri, body);
    }

    protected static ArianaLabException CreateHttpException(
        HttpStatusCode statusCode,
        string requestUri,
        string body,
        string? notFoundMessage = null)
    {
        return statusCode switch
        {
            HttpStatusCode.NotFound => new ArianaLabException(
                notFoundMessage ?? $"Resource not found: {requestUri}",
                statusCode),
            HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden => new ArianaLabException(
                "ArianaLab authentication failed. Check AraianLab credentials and BaseUrl configuration.",
                statusCode),
            _ => new ArianaLabException(
                $"ArianaLab request failed (HTTP {(int)statusCode} {statusCode}): {TruncateBody(body)}",
                statusCode),
        };
    }

    private static string TruncateBody(string body, int maxLength = 500)
    {
        if (string.IsNullOrWhiteSpace(body))
            return "(empty response body)";

        var trimmed = body.Trim();
        return trimmed.Length <= maxLength ? trimmed : $"{trimmed[..maxLength]}...";
    }
}
