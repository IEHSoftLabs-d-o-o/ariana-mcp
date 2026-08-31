using System.Net;
using System.Net.Http.Headers;
using Ariana_Mcp.integrations.Exceptions;

namespace Ariana_Mcp.Integrations.AraianLab;

public sealed class AraianLabAuthHandler(
    IArianaLabTokenService tokenService,
    IArianaLabRequestAuth? requestAuth = null) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var header = request.Headers.Authorization is { Parameter.Length: > 0 } existing
            ? $"{existing.Scheme} {existing.Parameter}"
            : requestAuth?.AuthorizationHeader;

        var validation = await tokenService.ValidateAuthorizationHeaderAsync(header).ConfigureAwait(false);
        if (validation.Status == ArianaLabTokenStatus.Expired)
        {
            throw new ArianaLabException(
                "The Bearer token has expired. Call POST /login again.",
                HttpStatusCode.Unauthorized);
        }

        if (!validation.IsValid || string.IsNullOrEmpty(validation.User) || string.IsNullOrEmpty(validation.Password))
        {
            throw new ArianaLabException(
                "Missing or invalid Bearer token. Call POST /login and send Authorization: Bearer <token>.",
                HttpStatusCode.Unauthorized);
        }

        var credentials = Convert.ToBase64String(
            System.Text.Encoding.UTF8.GetBytes($"{validation.User}:{validation.Password}"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}
