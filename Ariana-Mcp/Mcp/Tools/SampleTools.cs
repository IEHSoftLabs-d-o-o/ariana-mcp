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
        Title = "Proben suchen",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Sucht Laborproben nach Tagebuchnummer, Kunde, Kundenprobennummer, Probenbezeichnung, Zeitraum oder Status. " +
        "Verwenden, wenn der Nutzer eine Probe finden möchte und die genaue Tagebuchnummer nicht sicher bekannt ist.")]
    public Task<string> SearchSamples(
        [Description("Erweiterte EasyQuery-Suche als JSON oder Filterzeichenkette. Optional, wenn einfache Parameter genutzt werden.")]
        string? q = null,
        [Description("Tagebuchnummer oder Teil davon, z. B. '26-0318054'.")]
        string? tagebuchnummer = null,
        [Description("Kundenname oder Teil des Auftraggebers, z. B. 'Müller'.")]
        string? kunde = null,
        [Description("Kundenprobennummer oder Teil davon.")]
        string? kundenprobennummer = null,
        [Description("Probenbezeichnung oder Teil davon.")]
        string? probenbezeichnung = null,
        [Description("Eingangsdatum von (z. B. '2026-01-01').")]
        string? von = null,
        [Description("Eingangsdatum bis (z. B. '2026-03-31').")]
        string? bis = null,
        [Description("Status, z. B. 'fertiggemeldet', 'beurteilt', 'storniert'.")]
        string? status = null,
        [Description("Maximale Trefferanzahl (1-50, Standard 25).")]
        int limit = 25,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => sampleService.SearchSamplesAsync(
                q, tagebuchnummer, kunde, kundenprobennummer, probenbezeichnung, von, bis, status, limit,
                cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "get_sample",
        Title = "Probe laden",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Lädt die vollständigen Daten einer Probe anhand der Tagebuchnummer. " +
        "Nur verwenden, wenn Detaildaten gebraucht werden; für einen schnellen Überblick besser get_sample_short_info verwenden.")]
    public Task<string> GetSample(
        [Description("Tagebuchnummer der Probe, z. B. '26-0318054'.")]
        string tagebuchnummer,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => sampleService.GetSampleByIdAsync(tagebuchnummer, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "sample_by_id",
        Title = "Probe nach ID (Alias)",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Lädt eine oder mehrere Laborproben anhand ihrer Tagebuchnummern. Kompatibilitätsalias zu get_sample mit Batch-Unterstützung.")]
    public Task<string> SampleById(
        [Description("Liste von Tagebuchnummern, z. B. ['26-0318054', '26-0318055'].")]
        IReadOnlyList<string> sampleIds,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => sampleService.GetSamplesByIdsAsync(sampleIds, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "get_sample_short_info",
        Title = "Probe Kurzübersicht",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Gibt eine kurze Übersicht zu einer Probe zurück, z. B. Status, Verknüpfungen und wichtige Kopfdaten. " +
        "Dieses Tool bevorzugen, wenn der Nutzer allgemein fragt, was mit einer Probe ist.")]
    public Task<string> GetSampleShortInfo(
        [Description("Tagebuchnummer der Probe, z. B. '26-0318054'.")]
        string tagebuchnummer,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => sampleService.GetSampleShortInfoAsync(tagebuchnummer, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "report_json_by_sample",
        Title = "Prüfbericht JSON",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Lädt den strukturierten Prüfbericht zu einer Probe. " +
        "Verwenden, wenn der Nutzer Ergebnisse, Beurteilungen, Prüfberichtsdaten oder den Berichtsinhalt verstehen möchte.")]
    public Task<string> ReportJsonBySample(
        [Description("Tagebuchnummer der Probe, z. B. '26-0318054'.")]
        string tagebuchnummer,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => sampleService.GetReportJsonBySampleAsync(tagebuchnummer, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "customer_info_by_sample",
        Title = "Kunde zur Probe",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Lädt Kundeninformationen zum Auftraggeber einer Probe. " +
        "Verwenden, wenn der Nutzer zu einer Probe wissen möchte, welcher Kunde dazugehört oder welche Kundenhinweise gelten.")]
    public Task<string> CustomerInfoBySample(
        [Description("Tagebuchnummer der Probe, z. B. '26-0318054'.")]
        string tagebuchnummer,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => customerService.GetCustomerInfoBySampleAsync(tagebuchnummer, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "sample_results_by_id",
        Title = "Probenergebnisse laden",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Lädt die Bearbeitungs- und Ergebnisdaten einer Probe, inklusive Parameter, Methoden, Messwerte, Ergebnisse und Unterproben. " +
        "Verwenden, wenn der Nutzer konkrete Analyseergebnisse oder Parameter sehen möchte.")]
    public Task<string> SampleResultsById(
        [Description("Tagebuchnummer der Probe, z. B. '26-0318054'.")]
        string tagebuchnummer,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => sampleService.GetSampleResultsAsync(tagebuchnummer, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "get_sample_logs",
        Title = "Probe Änderungsprotokoll",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Lädt das Änderungsprotokoll zu einer Probe. Nur verwenden, wenn ausdrücklich gefragt wird, wer wann etwas geändert hat; " +
        "kann interne Auditdaten enthalten. Erfordert AraianLab:EnableSensitiveData=true.")]
    public Task<string> GetSampleLogs(
        [Description("Tagebuchnummer der Probe, z. B. '26-0318054'.")]
        string tagebuchnummer,
        SensitiveDataTools sensitiveDataTools,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(async () =>
        {
            sensitiveDataTools.EnsureSensitiveDataEnabled("Probe-Logs");
            return await sampleService.GetSampleLogsAsync(tagebuchnummer, cancellationToken);
        }, cancellationToken);
}
