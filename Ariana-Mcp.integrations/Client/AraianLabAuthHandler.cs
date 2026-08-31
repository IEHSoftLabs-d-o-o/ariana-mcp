using System.Net.Http.Headers;
using Microsoft.Extensions.Options;

namespace Ariana_Mcp.Integrations.AraianLab;

public sealed class AraianLabAuthHandler(
    IOptions<AraianLabClientOptions> options,
    IArianaLabRequestAuth? requestAuth = null) : DelegatingHandler
{
    private readonly AraianLabClientOptions _options = options.Value;

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = ResolveToken(request);
        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", token);

        return base.SendAsync(request, cancellationToken);
    }

    private string? ResolveToken(HttpRequestMessage request)
    {
        if (request.Headers.Authorization?.Parameter is { Length: > 0 } existing
            && ArianaLabBearerToken.TryRead(
                $"{request.Headers.Authorization.Scheme} {existing}",
                out _,
                out _,
                out var requestToken))
        {
            return requestToken;
        }

        if (ArianaLabBearerToken.TryRead(requestAuth?.AuthorizationHeader, out _, out _, out var incomingToken))
            return incomingToken;

        if (!string.IsNullOrEmpty(_options.User) && !string.IsNullOrEmpty(_options.Password))
            return ArianaLabBearerToken.Create(_options.User, _options.Password);

        return null;
    }
}
