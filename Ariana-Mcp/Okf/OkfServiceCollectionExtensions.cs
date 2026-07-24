using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ariana_Mcp.Okf;

internal static class OkfServiceCollectionExtensions
{
    public static IServiceCollection AddOkf(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<OkfOptions>()
            .Bind(configuration.GetSection(OkfOptions.SectionName))
            .ValidateDataAnnotations()
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.BundlePath),
                "Okf:BundlePath must be configured.")
            .ValidateOnStart();

        services.AddSingleton<OkfService>();
        return services;
    }
}
