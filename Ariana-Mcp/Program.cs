using System.Reflection;
using Ariana_Mcp.Configuration;
using Ariana_Mcp.Integrations.AraianLab;
using Ariana_Mcp.Mcp;
using Ariana_Mcp.Okf;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;

var argv = new List<string>(args);

var builder = WebApplication.CreateBuilder(argv.ToArray());

builder.ConfigureAppSettings(argv.ToArray());
builder.ConfigureLoggingSettings();

builder.Services.AddAraianLabHttpClient(builder.Configuration);
builder.Services.AddOkf(builder.Configuration);
builder.Services.AddOpenApi();

builder.Services
    .AddMcpServer(options =>
    {
        options.ServerInstructions = ServerInstructions.Text;
    })
    .WithHttpTransport(o => o.Stateless = true)
    .WithToolsFromAssembly()
    .WithResourcesFromAssembly();

var app = builder.Build();

// Tool types are activated lazily per call, so an unregistered dependency would only
// surface as a failed tool invocation. Activate each one once to fail fast at startup.
using (var scope = app.Services.CreateScope())
{
    foreach (var toolType in Assembly.GetExecutingAssembly().GetTypes()
        .Where(type => !type.IsAbstract && type.GetCustomAttribute<McpServerToolTypeAttribute>() is not null))
    {
        ActivatorUtilities.CreateInstance(scope.ServiceProvider, toolType);
    }
}

app.MapGet("/", () =>
{
    var asm = Assembly.GetExecutingAssembly();
    var name = asm.GetName().Name ?? "Ariana-Mcp";
    var version =
        asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
        ?? asm.GetName().Version?.ToString()
        ?? "unknown";

    return Results.Ok(new { name, version });
});

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapOpenApi("/openapi.json");
app.MapMcp("/mcp");

await app.RunAsync();
