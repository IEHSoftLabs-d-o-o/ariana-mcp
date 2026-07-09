using System.ComponentModel;
using Ariana_Mcp.integrations.Services;
using Ariana_Mcp.Mcp;
using ModelContextProtocol.Server;

namespace Ariana_Mcp.Mcp.Tools;

[McpServerToolType]
public sealed class CorTools(CorService corService, SensitiveDataTools sensitiveDataTools)
{
    [McpServerTool(Name = "search_cor", Title = "COR suchen", ReadOnly = true, Idempotent = true)]
    [Description(
        "Sucht Customer Order Requests nach erweiterten Suchkriterien. " +
        "Nur verwenden, wenn der Nutzer COR-Aufträge finden möchte. Erfordert AraianLab:EnableSensitiveData=true.")]
    public Task<string> SearchCor(
        [Description("Erweiterte EasyQuery-Suche als JSON. Optional.")]
        string? q = null,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(async () =>
        {
            sensitiveDataTools.EnsureSensitiveDataEnabled("COR-Suche");
            return await corService.SearchCorAsync(q, cancellationToken);
        }, cancellationToken);

    [McpServerTool(Name = "get_cor", Title = "COR laden", ReadOnly = true, Idempotent = true)]
    [Description(
        "Lädt einen Customer Order Request. Kann Rechnungs- und Zahlungsdaten enthalten. Erfordert AraianLab:EnableSensitiveData=true.")]
    public Task<string> GetCor(
        [Description("COR-ID.")]
        string corId,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(async () =>
        {
            sensitiveDataTools.EnsureSensitiveDataEnabled("COR-Details");
            return await corService.GetCorAsync(corId, cancellationToken);
        }, cancellationToken);

    [McpServerTool(Name = "validate_cor_gateway", Title = "COR Gateway prüfen", ReadOnly = true, Idempotent = true)]
    [Description(
        "Prüft einen COR-Gateway-Auftrag auf fachliche Plausibilität, ohne ihn zu speichern. " +
        "Nur verwenden, wenn der Nutzer ausdrücklich eine COR-Validierung möchte. Erfordert AraianLab:EnableSensitiveData=true.")]
    public Task<string> ValidateCorGateway(
        [Description("COR-Gateway-DTO als JSON.")]
        string dto,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(async () =>
        {
            sensitiveDataTools.EnsureSensitiveDataEnabled("COR-Validierung");
            return await corService.ValidateCorGatewayAsync(dto, cancellationToken);
        }, cancellationToken);
}
