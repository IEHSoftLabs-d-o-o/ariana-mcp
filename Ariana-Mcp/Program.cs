using System.Reflection;
using Ariana_Mcp.Configuration;
using Ariana_Mcp.Integrations.AraianLab;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

var argv = new List<string>(args);

var builder = WebApplication.CreateBuilder(argv.ToArray());

builder.ConfigureAppSettings(argv.ToArray());
builder.ConfigureLoggingSettings();

builder.Services.AddAraianLabHttpClient(builder.Configuration);

builder.Services
    .AddMcpServer(options =>
    {
        options.ServerInstructions =
            """
            Read-only access to ArianaLab (laboratory LIMS). German domain terms: Kunde = customer, Probe = sample.
            Typical workflow: search_customers (partial name) or customer_by_name (exact name) to find a customer,
            then customer_info_by_id for detailed information, and sample_by_id for samples.
            Sample ids use the format 'YY-NNNNNNN' (e.g. '26-0318054'). Customer ids are numeric (KundeId).
            customer_by_name requires an exact name match; prefer search_customers when you only have part of a name.
            """;
    })
    .WithHttpTransport(o => o.Stateless = true)
    .WithToolsFromAssembly();

var app = builder.Build();

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
app.MapMcp("/mcp");

await app.RunAsync();

