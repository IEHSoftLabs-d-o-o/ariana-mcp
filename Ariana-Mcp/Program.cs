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
            Lesender Zugriff auf ArianaLab (Labor-LIMS). Die Daten und Antworten sind auf Deutsch.
            Typischer Ablauf: search_customers (Teil des Namens) oder customer_by_name (exakter Name), um einen Kunden zu finden,
            danach customer_info_by_id für Detailinformationen und sample_by_id für Proben.
            Alle Tools akzeptieren Listen von Eingaben und liefern pro Eintrag ein eigenes Ergebnis; fehlende Einträge brechen die gesamte Anfrage nicht ab.
            Proben-IDs haben das Format 'JJ-NNNNNNN' (z. B. '26-0318054'). Kunden-IDs sind numerisch (KundeId).
            customer_by_name erfordert einen exakt passenden Namen; bei nur teilweise bekanntem Namen bitte search_customers verwenden.
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

