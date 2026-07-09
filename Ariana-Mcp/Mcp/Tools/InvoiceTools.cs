using System.ComponentModel;
using Ariana_Mcp.integrations.Services;
using Ariana_Mcp.Mcp;
using ModelContextProtocol.Server;

namespace Ariana_Mcp.Mcp.Tools;

[McpServerToolType]
public sealed class InvoiceTools(InvoiceService invoiceService, SensitiveDataTools sensitiveDataTools)
{
    [McpServerTool(Name = "search_invoices", Title = "Rechnungen suchen", ReadOnly = true, Idempotent = true)]
    [Description(
        "Sucht Rechnungen. Nur verwenden, wenn der Nutzer ausdrücklich nach Rechnung, Rechnungsnummer oder Abrechnung fragt; " +
        "enthält sensible Abrechnungsdaten. Erfordert AraianLab:EnableSensitiveData=true.")]
    public Task<string> SearchInvoices(
        [Description("Erweiterte EasyQuery-Suche als JSON. Optional.")]
        string? q = null,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(async () =>
        {
            sensitiveDataTools.EnsureSensitiveDataEnabled("Rechnungssuche");
            return await invoiceService.SearchInvoicesAsync(q, cancellationToken);
        }, cancellationToken);

    [McpServerTool(Name = "get_invoice", Title = "Rechnung laden", ReadOnly = true, Idempotent = true)]
    [Description(
        "Lädt eine Rechnung anhand der Rechnungsnummer. Enthält abrechnungsrelevante Daten. Erfordert AraianLab:EnableSensitiveData=true.")]
    public Task<string> GetInvoice(
        [Description("Rechnungsnummer oder ID.")]
        string id,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(async () =>
        {
            sensitiveDataTools.EnsureSensitiveDataEnabled("Rechnungsdetails");
            return await invoiceService.GetInvoiceAsync(id, cancellationToken);
        }, cancellationToken);
}
