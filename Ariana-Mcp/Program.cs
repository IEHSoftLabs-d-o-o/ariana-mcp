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
            Lesender Zugriff auf ArianaLab (Labor-LIMS). Daten und Antworten sind auf Deutsch.

            Typischer DAUS-Ablauf:
            1. search_customers (Name/Nummer) oder search_samples (Tagebuchnummer, Kunde, Zeitraum) zum Finden
            2. get_sample_short_info für einen schnellen Überblick zu einer Probe
            3. customer_info_by_sample für Kundenkontext zur Probe
            4. sample_results_by_id für konkrete Parameter und Messwerte
            5. report_json_by_sample für Prüfberichtsinhalt und Beurteilungen

            Proben-IDs haben das Format 'JJ-NNNNNNN' (z. B. '26-0318054'). Kundennummern sind numerisch (z. B. '14197').
            Für Detaildaten einer bekannten Probe: get_sample oder Resource arianalab://sample/{tagebuchnummer}.
            Für Kundenstammdaten: arianalab://customer/{nummer} oder customer_info_by_id.

            Referenzdaten: get_public_analyses, get_methods, get_product_classes, list_lab_parameters, list_units,
            list_product_groups, list_sample_groups, list_test_packages.

            Aufträge: search_orders, get_order, search_customer_orders, get_customer_order, get_planning_orders.

            Diagnose: get_system_info prüft Erreichbarkeit und angemeldeten Benutzer.

            Sensible Daten (Logs, Anhänge, Rechnungen, COR) sind standardmäßig gesperrt.
            Nur bei ausdrücklichem Bedarf und mit AraianLab:EnableSensitiveData=true verwenden.

            Batch-Tools (sample_by_id, customer_by_name, search_customers_batch) akzeptieren Listen;
            fehlende Einträge brechen die gesamte Anfrage nicht ab.
            """;
    })
    .WithHttpTransport(o => o.Stateless = true)
    .WithToolsFromAssembly()
    .WithResourcesFromAssembly();

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
