using Ariana_Mcp.integrations.Helpers;
using Ariana_Mcp.integrations.Services;
using Microsoft.Extensions.Configuration;using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Ariana_Mcp.Integrations.AraianLab;

public static class ServiceCollectionExtensions
{
    private static readonly TimeSpan HttpClientTimeout = TimeSpan.FromMinutes(3);

    public static IServiceCollection AddAraianLabHttpClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<AraianLabClientOptions>()
            .Bind(configuration.GetSection(AraianLabClientOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<IArianaLabTokenService, ArianaLabTokenService>();
        services.AddTransient<AraianLabAuthHandler>();
        services.AddTransient<CustomerService>();
        services.AddTransient<SampleService>();
        services.AddTransient<ReferenceDataService>();
        services.AddTransient<OrderService>();
        services.AddTransient<SystemService>();
        services.AddTransient<CorService>();
        services.AddHttpClient(ArianaLabHttp.ClientName, (sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<AraianLabClientOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
                client.Timeout = HttpClientTimeout;
            })
            .AddHttpMessageHandler<AraianLabAuthHandler>();

        return services;
    }
}
