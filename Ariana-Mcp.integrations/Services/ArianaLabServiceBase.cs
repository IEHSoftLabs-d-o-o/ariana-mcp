using System.Net;
using System.Text;
using System.Text.Json;
using Ariana_Mcp.Integrations.AraianLab;
using Ariana_Mcp.integrations.Exceptions;
using Ariana_Mcp.integrations.Helpers;

namespace Ariana_Mcp.integrations.Services;

public abstract class ArianaLabServiceBase(IHttpClientFactory httpClientFactory)
{
    protected HttpClient CreateClient() => httpClientFactory.CreateClient(ArianaLabHttp.ClientName);

    protected Task<string> GetAsync(string requestUri, CancellationToken cancellationToken = default) =>
        GetAsStringAsync(CreateClient(), requestUri, cancellationToken);

    protected Task<string> GetQueryAsync(
        string requestUri,
        string? q,
        CancellationToken cancellationToken = default)
    {
        var uri = EasyQueryBuilder.AppendQueryParameter(requestUri, q);
        return GetAsync(uri, cancellationToken);
    }

    protected async Task<string> PostAsStringAsync(
        string requestUri,
        object? body,
        CancellationToken cancellationToken = default)
    {
        var client = CreateClient();
        using var content = body is null
            ? null
            : new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

        using var response = content is null
            ? await client.PostAsync(requestUri, null, cancellationToken).ConfigureAwait(false)
            : await client.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
            return responseBody;

        throw CreateHttpException(response.StatusCode, requestUri, responseBody);
    }

    protected static async Task<string> PostJsonAsStringAsync(
        HttpClient client,
        string requestUri,
        string json,
        CancellationToken cancellationToken)
    {
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        using var response = await client.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
            return responseBody;

        throw CreateHttpException(response.StatusCode, requestUri, responseBody);
    }

    protected static string WithQuery(string requestUri, params (string Name, string? Value)[] parameters)
    {
        var query = parameters
            .Where(parameter => !string.IsNullOrWhiteSpace(parameter.Value))
            .Select(parameter => $"{Uri.EscapeDataString(parameter.Name)}={Uri.EscapeDataString(parameter.Value!)}")
            .ToArray();

        if (query.Length == 0)
            return requestUri;

        var separator = requestUri.Contains('?') ? '&' : '?';
        return $"{requestUri}{separator}{string.Join('&', query)}";
    }

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
                notFoundMessage ?? $"The requested ArianaLab data was not found: {requestUri}",
                statusCode),
            HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden => new ArianaLabException(
                "ArianaLab denied access. Check whether the MCP user is signed in and has the required permission for this data.",
                statusCode),
            _ => new ArianaLabException(
                $"ArianaLab could not answer the request right now (HTTP {(int)statusCode} {statusCode}). Response: {TruncateBody(body)}",
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
