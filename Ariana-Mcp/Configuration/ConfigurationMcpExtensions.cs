using System.Reflection;
using Ariana_Mcp.Mcp;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;

namespace Ariana_Mcp.Configuration;

internal static class ConfigurationMcpExtensions
{
    internal static WebApplicationBuilder ConfigureMcp(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddMcpServer(options =>
            {
                options.ServerInstructions = ServerInstructions.Text;
            })
            .WithHttpTransport(o => o.Stateless = true)
            .WithToolsFromAssembly()
            .WithResourcesFromAssembly();

        return builder;
    }

    internal static WebApplication UseConfiguredMcp(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            foreach (var toolType in Assembly.GetExecutingAssembly().GetTypes()
                         .Where(type => !type.IsAbstract && type.GetCustomAttribute<McpServerToolTypeAttribute>() is not null))
            {
                ActivatorUtilities.CreateInstance(scope.ServiceProvider, toolType);
            }
        }

        app.MapMcp("/mcp");
        return app;
    }
}
