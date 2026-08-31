using System.Net;
using System.Net.Http.Headers;
using Ariana_Mcp.integrations.Exceptions;

namespace Ariana_Mcp.Integrations.AraianLab;

public sealed class AraianLabAuthHandler(IArianaLabRequestAuth? requestAuth = null) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var header = request.Headers.Authorization is { Parameter.Length: > 0 } existing
            ? $"{existing.Scheme} {existing.Parameter}"
            : requestAuth?.AuthorizationHeader;

        if (!ArianaLabBearerToken.TryParse(header, out var token) || token is null)
        {
            throw new ArianaLabException(
                "Missing or invalid Bearer token. Call POST /login and send Authorization: Bearer <token>.",
                HttpStatusCode.Unauthorized);
        }

        if (token.IsExpired)
        {
            throw new ArianaLabException(
                "The Bearer token has expired. Call POST /login again.",
                HttpStatusCode.Unauthorized);
        }

        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", token.Credentials);
        return base.SendAsync(request, cancellationToken);
    }
}
