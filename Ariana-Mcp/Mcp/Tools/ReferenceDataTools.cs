using System.ComponentModel;
using Ariana_Mcp.integrations.Services;
using Ariana_Mcp.Mcp;
using ModelContextProtocol.Server;

namespace Ariana_Mcp.Mcp.Tools;

[McpServerToolType]
public sealed class ReferenceDataTools(ReferenceDataService referenceDataService)
{
    [McpServerTool(Name = "search_analyses", Title = "Analysen suchen", ReadOnly = true, Idempotent = true)]
    [Description(
        "Sucht Analysen oder Untersuchungen nach Name, Beschreibung oder Stichwort. " +
        "Mit includeTechnical können technische/interne Analysen einbezogen werden, falls erlaubt.")]
    public Task<string> SearchAnalyses(
        [Description("Suchbegriff, z. B. 'Salmonellen' oder 'Keimzahl'.")]
        string q,
        [Description("Technische/interne Analysen einbeziehen.")]
        bool includeTechnical = false,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => referenceDataService.SearchAnalysesAsync(q, includeTechnical, cancellationToken: cancellationToken),
            cancellationToken);

    [McpServerTool(Name = "get_public_analyses", Title = "Öffentlicher Analysekatalog", ReadOnly = true, Idempotent = true)]
    [Description(
        "Lädt den öffentlichen Analysekatalog. Verwenden, wenn der Nutzer wissen möchte, welche Analysen grundsätzlich verfügbar sind.")]
    public Task<string> GetPublicAnalyses(CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => referenceDataService.GetPublicAnalysesAsync(cancellationToken),
            cancellationToken);

    [McpServerTool(Name = "get_methods", Title = "Prüfmethoden laden", ReadOnly = true, Idempotent = true)]
    [Description(
        "Lädt Prüfmethoden. Verwenden, wenn der Nutzer nach Methoden, Untersuchungsverfahren oder passenden Methoden zu Analysen fragt.")]
    public Task<string> GetMethods(
        [Description("Nur Namensliste statt vollständiger HAL-Antwort.")]
        bool plainList = false,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => referenceDataService.GetMethodsAsync(plainList, cancellationToken),
            cancellationToken);

    [McpServerTool(Name = "get_product_classes", Title = "Produktklassen laden", ReadOnly = true, Idempotent = true)]
    [Description(
        "Lädt Produktklassen oder Warengruppen. Verwenden, wenn eine Probe, Analyse oder Beauftragung einer Produktklasse zugeordnet werden soll.")]
    public Task<string> GetProductClasses(
        [Description("Kontext: 'public' (Standard), 'mad' oder 'cor'.")]
        string context = "public",
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => referenceDataService.GetProductClassesAsync(context, cancellationToken),
            cancellationToken);

    [McpServerTool(Name = "list_lab_parameters", Title = "Laborparameter suchen", ReadOnly = true, Idempotent = true)]
    [Description(
        "Sucht Laborparameter oder Analyten, z. B. Keimzahl, pH-Wert oder Salmonellen. Verwenden, wenn ein Parametername unklar ist.")]
    public Task<string> ListLabParameters(
        [Description("Suchbegriff, z. B. 'pH' oder 'Keimzahl'.")]
        string? term = null,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => referenceDataService.ListLabParametersAsync(term, cancellationToken: cancellationToken),
            cancellationToken);

    [McpServerTool(Name = "list_units", Title = "Einheiten laden", ReadOnly = true, Idempotent = true)]
    [Description(
        "Lädt Nachschlageliste für Einheiten. Verwenden, wenn Begriffe aus Probe, Auftrag oder Prüfbericht erklärt oder abgeglichen werden sollen.")]
    public Task<string> ListUnits(CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => referenceDataService.ListUnitsAsync(cancellationToken),
            cancellationToken);

    [McpServerTool(Name = "list_product_groups", Title = "Produktgruppen laden", ReadOnly = true, Idempotent = true)]
    [Description("Lädt Nachschlageliste für Produktgruppen.")]
    public Task<string> ListProductGroups(
        [Description("Optionaler Suchbegriff.")]
        string? term = null,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => referenceDataService.ListProductGroupsAsync(term, cancellationToken),
            cancellationToken);

    [McpServerTool(Name = "list_sample_groups", Title = "Probengruppen laden", ReadOnly = true, Idempotent = true)]
    [Description("Lädt Nachschlageliste für Probengruppen.")]
    public Task<string> ListSampleGroups(
        [Description("Optionaler Suchbegriff.")]
        string? term = null,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => referenceDataService.ListSampleGroupsAsync(term, cancellationToken),
            cancellationToken);

    [McpServerTool(Name = "list_test_packages", Title = "Prüfpakete laden", ReadOnly = true, Idempotent = true)]
    [Description("Lädt Nachschlageliste für Prüfpakete.")]
    public Task<string> ListTestPackages(
        [Description("Optionaler Suchbegriff.")]
        string? term = null,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => referenceDataService.ListTestPackagesAsync(term, cancellationToken: cancellationToken),
            cancellationToken);
}
