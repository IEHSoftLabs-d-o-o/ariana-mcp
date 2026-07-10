using System.ComponentModel;
using Ariana_Mcp.integrations.Services;
using Ariana_Mcp.Mcp;
using ModelContextProtocol.Server;

namespace Ariana_Mcp.Mcp.Tools;

[McpServerToolType]
public sealed class InvoiceTools(InvoiceService invoiceService, SensitiveDataTools sensitiveDataTools)
{
    [McpServerTool(Name = "search_invoices", Title = "Search invoices", ReadOnly = true, Idempotent = true)]
    [Description(
        "Searches invoices. Use only when the user explicitly asks for an invoice, invoice number, or billing; " +
        "contains sensitive billing data. Requires AraianLab:EnableSensitiveData=true.")]
    public Task<string> SearchInvoices(
        [Description("Advanced EasyQuery search as JSON. Optional.")]
        string? q = null,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(async () =>
        {
            sensitiveDataTools.EnsureSensitiveDataEnabled("invoice search");
            return await invoiceService.SearchInvoicesAsync(q, cancellationToken);
        }, cancellationToken);

    [McpServerTool(Name = "get_invoice", Title = "Load invoice", ReadOnly = true, Idempotent = true)]
    [Description(
        "Loads an invoice by invoice number. Contains billing-relevant data. Requires AraianLab:EnableSensitiveData=true.")]
    public Task<string> GetInvoice(
        [Description("Invoice number or ID.")]
        string id,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(async () =>
        {
            sensitiveDataTools.EnsureSensitiveDataEnabled("invoice details");
            return await invoiceService.GetInvoiceAsync(id, cancellationToken);
        }, cancellationToken);
}
