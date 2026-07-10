using System.ComponentModel;
using Ariana_Mcp.integrations.Services;
using Ariana_Mcp.Mcp;
using ModelContextProtocol.Server;

namespace Ariana_Mcp.Mcp.Tools;

[McpServerToolType]
public sealed class CorTools(CorService corService, SensitiveDataTools sensitiveDataTools)
{
    [McpServerTool(Name = "search_cor", Title = "Search CORs", ReadOnly = true, Idempotent = true)]
    [Description(
        "Searches Customer Order Requests by advanced search criteria. " +
        "Use only when the user wants to find COR orders. Requires AraianLab:EnableSensitiveData=true.")]
    public Task<string> SearchCor(
        [Description("Advanced EasyQuery search as JSON. Optional.")]
        string? q = null,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(async () =>
        {
            sensitiveDataTools.EnsureSensitiveDataEnabled("COR search");
            return await corService.SearchCorAsync(q, cancellationToken);
        }, cancellationToken);

    [McpServerTool(Name = "get_cor", Title = "Load COR", ReadOnly = true, Idempotent = true)]
    [Description(
        "Loads a Customer Order Request. May contain invoice and payment data. Requires AraianLab:EnableSensitiveData=true.")]
    public Task<string> GetCor(
        [Description("COR-ID.")]
        string corId,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(async () =>
        {
            sensitiveDataTools.EnsureSensitiveDataEnabled("COR details");
            return await corService.GetCorAsync(corId, cancellationToken);
        }, cancellationToken);

    [McpServerTool(Name = "validate_cor_gateway", Title = "Validate COR gateway", ReadOnly = true, Idempotent = true)]
    [Description(
        "Checks a COR gateway order for business plausibility without saving it. " +
        "Use only when the user explicitly wants COR validation. Requires AraianLab:EnableSensitiveData=true.")]
    public Task<string> ValidateCorGateway(
        [Description("COR gateway DTO as JSON.")]
        string dto,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(async () =>
        {
            sensitiveDataTools.EnsureSensitiveDataEnabled("COR validation");
            return await corService.ValidateCorGatewayAsync(dto, cancellationToken);
        }, cancellationToken);
}
