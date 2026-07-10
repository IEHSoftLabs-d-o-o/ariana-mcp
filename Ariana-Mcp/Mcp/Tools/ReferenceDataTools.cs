using System.ComponentModel;
using Ariana_Mcp.integrations.Services;
using Ariana_Mcp.Mcp;
using ModelContextProtocol.Server;

namespace Ariana_Mcp.Mcp.Tools;

[McpServerToolType]
public sealed class ReferenceDataTools(ReferenceDataService referenceDataService)
{
    [McpServerTool(Name = "search_analyses", Title = "Search analyses", ReadOnly = true, Idempotent = true)]
    [Description(
        "Searches analyses or tests by name, description, or keyword. " +
        "includeTechnical can include technical/internal analyses when allowed.")]
    public Task<string> SearchAnalyses(
        [Description("Search term, for example 'Salmonellen' or 'Keimzahl'.")]
        string q,
        [Description("Include technical/internal analyses.")]
        bool includeTechnical = false,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => referenceDataService.SearchAnalysesAsync(q, includeTechnical, cancellationToken: cancellationToken),
            cancellationToken);

    [McpServerTool(Name = "get_public_analyses", Title = "Public analysis catalog", ReadOnly = true, Idempotent = true)]
    [Description(
        "Loads the public analysis catalog. Use when the user wants to know which analyses are generally available.")]
    public Task<string> GetPublicAnalyses(CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => referenceDataService.GetPublicAnalysesAsync(cancellationToken),
            cancellationToken);

    [McpServerTool(Name = "get_methods", Title = "Load test methods", ReadOnly = true, Idempotent = true)]
    [Description(
        "Loads test methods. Use when the user asks for methods, test procedures, or suitable methods for analyses.")]
    public Task<string> GetMethods(
        [Description("Return only the name list instead of the full HAL response.")]
        bool plainList = false,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => referenceDataService.GetMethodsAsync(plainList, cancellationToken),
            cancellationToken);

    [McpServerTool(Name = "get_product_classes", Title = "Load product classes", ReadOnly = true, Idempotent = true)]
    [Description(
        "Loads product classes or commodity groups. Use when a sample, analysis, or request should be assigned to a product class.")]
    public Task<string> GetProductClasses(
        [Description("Context: 'public' (default), 'mad', or 'cor'.")]
        string context = "public",
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => referenceDataService.GetProductClassesAsync(context, cancellationToken),
            cancellationToken);

    [McpServerTool(Name = "list_lab_parameters", Title = "Search lab parameters", ReadOnly = true, Idempotent = true)]
    [Description(
        "Searches lab parameters or analytes, for example Keimzahl, pH value, or Salmonella. Use when a parameter name is unclear.")]
    public Task<string> ListLabParameters(
        [Description("Search term, for example 'pH' or 'Keimzahl'.")]
        string? term = null,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => referenceDataService.ListLabParametersAsync(term, cancellationToken: cancellationToken),
            cancellationToken);

    [McpServerTool(Name = "list_units", Title = "Load units", ReadOnly = true, Idempotent = true)]
    [Description(
        "Loads the lookup list for units. Use when terms from a sample, order, or test report need to be explained or matched.")]
    public Task<string> ListUnits(CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => referenceDataService.ListUnitsAsync(cancellationToken),
            cancellationToken);

    [McpServerTool(Name = "list_product_groups", Title = "Load product groups", ReadOnly = true, Idempotent = true)]
    [Description("Loads the lookup list for product groups.")]
    public Task<string> ListProductGroups(
        [Description("Optional search term.")]
        string? term = null,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => referenceDataService.ListProductGroupsAsync(term, cancellationToken),
            cancellationToken);

    [McpServerTool(Name = "list_sample_groups", Title = "Load sample groups", ReadOnly = true, Idempotent = true)]
    [Description("Loads the lookup list for sample groups.")]
    public Task<string> ListSampleGroups(
        [Description("Optional search term.")]
        string? term = null,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => referenceDataService.ListSampleGroupsAsync(term, cancellationToken),
            cancellationToken);

    [McpServerTool(Name = "list_test_packages", Title = "Load test packages", ReadOnly = true, Idempotent = true)]
    [Description("Loads the lookup list for test packages.")]
    public Task<string> ListTestPackages(
        [Description("Optional search term.")]
        string? term = null,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => referenceDataService.ListTestPackagesAsync(term, cancellationToken: cancellationToken),
            cancellationToken);
}
