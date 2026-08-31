using Ariana_Mcp.Integrations.AraianLab;
using Microsoft.AspNetCore.Http;

namespace Ariana_Mcp.Auth;

public sealed class HttpContextArianaLabRequestAuth(IHttpContextAccessor httpContextAccessor)
    : IArianaLabRequestAuth
{
    public string? AuthorizationHeader
    {
        get
        {
            var header = httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();
            return string.IsNullOrWhiteSpace(header) ? null : header;
        }
    }
}
