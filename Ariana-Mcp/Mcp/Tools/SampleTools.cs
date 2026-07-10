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
        "Searches lab samples by lab journal number, customer, customer sample number, sample description, date range, or status. " +
        "Use when the user wants to find a sample and the exact lab journal number is not known.")]
    public Task<string> SearchSamples(
        [Description("Advanced EasyQuery search as JSON or a filter string. Optional when simple parameters are used.")]
        string? q = null,
        [Description("Lab journal number or part of it, for example '26-0318054'.")]
        string? tagebuchnummer = null,
        [Description("Customer name or part of the requester name, for example 'Müller'.")]
        string? kunde = null,
        [Description("Customer sample number or part of it.")]
        string? kundenprobennummer = null,
        [Description("Sample description or part of it.")]
        string? probenbezeichnung = null,
        [Description("Received date from, for example '2026-01-01'.")]
        string? von = null,
        [Description("Received date through, for example '2026-03-31'.")]
        string? bis = null,
        [Description("Status, for example 'fertiggemeldet', 'beurteilt', or 'storniert'.")]
        string? status = null,
        [Description("Maximum number of matches (1-50, default 25).")]
        int limit = 25,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => sampleService.SearchSamplesAsync(
                q, tagebuchnummer, kunde, kundenprobennummer, probenbezeichnung, von, bis, status, limit,
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
        "Returns a brief overview for a sample, for example status, links, and important header data. " +
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
        "may contain internal audit data. Requires AraianLab:EnableSensitiveData=true.")]
    public Task<string> GetSampleLogs(
        [Description("Sample lab journal number, for example '26-0318054'.")]
        string tagebuchnummer,
        SensitiveDataTools sensitiveDataTools,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(async () =>
        {
            sensitiveDataTools.EnsureSensitiveDataEnabled("sample logs");
            return await sampleService.GetSampleLogsAsync(tagebuchnummer, cancellationToken);
        }, cancellationToken);
}
