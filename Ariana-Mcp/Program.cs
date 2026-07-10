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
builder.Services.AddOpenApi();

builder.Services
    .AddMcpServer(options =>
    {
        options.ServerInstructions =
            """
            Read-only access to ArianaLab (labor LIMS). Data and responses are in German.

            Typical DAUS workflow:
            1. Use search_customers (name/number) or search_samples (lab journal number, customer, date range) to find records
            2. Use get_sample_short_info for a quick sample overview
            3. Use customer_info_by_sample for customer context for the sample
            4. Use sample_results_by_id for specific parameters and measured values
            5. Use report_json_by_sample for test report content and assessments

            Sample IDs use the format 'YY-NNNNNNN' (for example '26-0318054'). Customer numbers are numeric (for example '14197').
            For details of a known sample: get_sample or resource arianalab://sample/{tagebuchnummer}.
            For customer master data: arianalab://customer/{nummer} or customer_info_by_id.

            Reference data: get_public_analyses, get_methods, get_product_classes, list_lab_parameters, list_units,
            list_product_groups, list_sample_groups, list_test_packages.

            Orders: search_orders, get_order, search_customer_orders, get_customer_order, get_planning_orders.

            Diagnostics: get_system_info checks reachability and the signed-in user.

            Sensitive data (logs, attachments, invoices, COR) is blocked by default.
            Use only when explicitly needed and with AraianLab:EnableSensitiveData=true.

            Batch tools (sample_by_id, customer_by_name, search_customers_batch) accept lists;
            missing entries do not fail the entire request.
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

app.MapOpenApi("/openapi.json");
app.MapMcp("/mcp");

await app.RunAsync();
