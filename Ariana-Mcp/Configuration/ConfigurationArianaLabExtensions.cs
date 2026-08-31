using Ariana_Mcp.Auth;
using Ariana_Mcp.Integrations.AraianLab;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ariana_Mcp.Configuration;

internal static class ConfigurationArianaLabExtensions
{
    internal static WebApplicationBuilder ConfigureArianaLab(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddTransient<IArianaLabRequestAuth, HttpContextArianaLabRequestAuth>();
        builder.Services.AddAraianLabHttpClient(builder.Configuration);
        return builder;
    }
}
