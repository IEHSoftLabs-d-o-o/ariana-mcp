using System.ComponentModel;
using Ariana_Mcp.integrations.Services;
using Ariana_Mcp.Mcp;
using ModelContextProtocol.Server;

namespace Ariana_Mcp.Mcp.Tools;

[McpServerToolType]
public sealed class SampleTools(SampleService sampleService, CustomerService customerService)
{
    [McpServerTool(
        Name = "search_samples",
        Title = "Search samples",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Searches lab samples using the fields from the ArianaLab sample-search form. " +
        "Use when the user wants to find a sample and the exact lab journal number is not known.")]
    public Task<string> SearchSamples(
        [Description("Lab journal number or part of it, for example '26-0318054'.")]
        string? tagebuchnummer = null,
        [Description("Auftraggeber: customer name or number.")]
        string? auftraggeber = null,
        [Description("Rechnungsempfänger: invoice recipient name.")]
        string? rechnungsempfaenger = null,
        [Description("Customer sample number or part of it.")]
        string? kundenprobennummer = null,
        [Description("Sample description or part of it.")]
        string? probenbezeichnung = null,
        [Description("Fertiggemeldet am, from date in yyyy-MM-dd format.")]
        string? fertiggemeldetAmVon = null,
        [Description("Fertiggemeldet am, through date in yyyy-MM-dd format.")]
        string? fertiggemeldetAmBis = null,
        [Description("User who reported the sample complete.")]
        string? fertiggemeldetVon = null,
        [Description("Kategorie.")]
        string? kategorie = null,
        [Description("Prüfkategorie.")]
        string? pruefkategorie = null,
        [Description("Zusatzfelder as [{ Bezeichnung, Operator, Inhalt }]. Operator defaults to ~*. Example: [{ Bezeichnung: 'Artikelnummer', Operator: '~*', Inhalt: '18903' }].")]
        IReadOnlyList<SampleAdditionalFieldFilter>? zusatzfelder = null,
        [Description("Hauptprobe: true=yes, false=no, null=all. Defaults to true.")]
        bool? hauptprobe = true,
        [Description("Etikettiert: true=yes, false=no, null=all.")]
        bool? etikettiert = null,
        [Description("Erledigt: true=yes, false=no, null=all.")]
        bool? erledigt = null,
        [Description("Beurteilt: true=yes, false=no, null=all.")]
        bool? beurteilt = null,
        [Description("Geprüft: true=yes, false=no, null=all.")]
        bool? geprueft = null,
        [Description("Fertiggemeldet: true=yes, false=no, null=all.")]
        bool? fertiggemeldet = null,
        [Description("Archiviert: true=yes, false=no, null=all.")]
        bool? archiviert = null,
        [Description("Storniert: true=yes, false=no, null=all. Defaults to false.")]
        bool? storniert = false,
        [Description("Freigabe: true=yes, false=no, null=all.")]
        bool? freigabe = null,
        [Description("Eilig: true=yes, false=no, null=all.")]
        bool? eilig = null,
        [Description("Fakturiert: true=yes, false=no, null=all.")]
        bool? fakturiert = null,
        [Description("Prüfpakete, for example ['Mikrobiologie', 'Chemie'].")]
        IReadOnlyList<string>? pruefpakete = null,
        [Description("Kundengruppe 1.")]
        string? kundengruppe1 = null,
        [Description("Probengruppen, for example ['Planproben'].")]
        IReadOnlyList<string>? probengruppen = null,
        [Description("Produktgruppe.")]
        string? produktgruppe = null,
        [Description("Probeneingang, from date in yyyy-MM-dd format.")]
        string? probeneingangVon = null,
        [Description("Probeneingang, through date in yyyy-MM-dd format.")]
        string? probeneingangBis = null,
        [Description("Termin, from date in yyyy-MM-dd format.")]
        string? terminVon = null,
        [Description("Termin, through date in yyyy-MM-dd format.")]
        string? terminBis = null,
        [Description("Erfasst am, from date in yyyy-MM-dd format.")]
        string? erfasstAmVon = null,
        [Description("Erfasst am, through date in yyyy-MM-dd format.")]
        string? erfasstAmBis = null,
        [Description("User who created the sample.")]
        string? erfasstVon = null,
        [Description("Etikettiert am, from date in yyyy-MM-dd format.")]
        string? etikettiertAmVon = null,
        [Description("Etikettiert am, through date in yyyy-MM-dd format.")]
        string? etikettiertAmBis = null,
        [Description("User who labeled the sample.")]
        string? etikettiertVon = null,
        [Description("Abteilungen, for example ['Mibi'] or ['Chemie', 'Mibi'].")]
        IReadOnlyList<string>? abteilungen = null,
        [Description("Maximum number of matches (1-100, default 25).")]
        int limit = 25,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => sampleService.SearchSamplesAsync(
                tagebuchnummer,
                auftraggeber,
                rechnungsempfaenger,
                kundenprobennummer,
                probenbezeichnung,
                fertiggemeldetAmVon,
                fertiggemeldetAmBis,
                fertiggemeldetVon,
                kategorie,
                pruefkategorie,
                zusatzfelder,
                hauptprobe,
                etikettiert,
                erledigt,
                beurteilt,
                geprueft,
                fertiggemeldet,
                archiviert,
                storniert,
                freigabe,
                eilig,
                fakturiert,
                pruefpakete,
                kundengruppe1,
                probengruppen,
                produktgruppe,
                probeneingangVon,
                probeneingangBis,
                terminVon,
                terminBis,
                erfasstAmVon,
                erfasstAmBis,
                erfasstVon,
                etikettiertAmVon,
                etikettiertAmBis,
                etikettiertVon,
                abteilungen,
                limit,
                cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "get_sample",
        Title = "Load sample",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Loads the full data for a sample by lab journal number. " +
        "Use only when detailed data is needed; prefer get_sample_short_info for a quick overview.")]
    public Task<string> GetSample(
        [Description("Sample lab journal number, for example '26-0318054'.")]
        string tagebuchnummer,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => sampleService.GetSampleByIdAsync(tagebuchnummer, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "sample_by_id",
        Title = "Sample by ID (alias)",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Loads one or more lab samples by their lab journal numbers. Compatibility alias for get_sample with batch support.")]
    public Task<string> SampleById(
        [Description("List of lab journal numbers, for example ['26-0318054', '26-0318055'].")]
        IReadOnlyList<string> sampleIds,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => sampleService.GetSamplesByIdsAsync(sampleIds, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "get_sample_short_info",
        Title = "Sample quick overview",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Returns a brief overview for a sample, for example status and important header data. " +
        "Prefer this tool when the user asks generally what is happening with a sample.")]
    public Task<string> GetSampleShortInfo(
        [Description("Sample lab journal number, for example '26-0318054'.")]
        string tagebuchnummer,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => sampleService.GetSampleShortInfoAsync(tagebuchnummer, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "report_json_by_sample",
        Title = "Test report JSON",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Loads the structured test report for a sample. " +
        "Use when the user wants to understand results, assessments, report data, or report content.")]
    public Task<string> ReportJsonBySample(
        [Description("Sample lab journal number, for example '26-0318054'.")]
        string tagebuchnummer,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => sampleService.GetReportJsonBySampleAsync(tagebuchnummer, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "customer_info_by_sample",
        Title = "Customer for sample",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Loads customer information for a sample's requester. " +
        "Use when the user wants to know which customer belongs to a sample or which customer notes apply.")]
    public Task<string> CustomerInfoBySample(
        [Description("Sample lab journal number, for example '26-0318054'.")]
        string tagebuchnummer,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => customerService.GetCustomerInfoBySampleAsync(tagebuchnummer, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "sample_results_by_id",
        Title = "Load sample results",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Loads processing and result data for a sample, including parameters, methods, measured values, results, and subsamples. " +
        "Use when the user wants to see specific analysis results or parameters.")]
    public Task<string> SampleResultsById(
        [Description("Sample lab journal number, for example '26-0318054'.")]
        string tagebuchnummer,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => sampleService.GetSampleResultsAsync(tagebuchnummer, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "get_sample_logs",
        Title = "Sample change log",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Loads the change log for a sample. Use only when explicitly asked who changed something and when; " +
        "may contain internal audit data.")]
    public Task<string> GetSampleLogs(
        [Description("Sample lab journal number, for example '26-0318054'.")]
        string tagebuchnummer,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => sampleService.GetSampleLogsAsync(tagebuchnummer, cancellationToken),
            cancellationToken);
}
