using System.ComponentModel;
using Ariana_Mcp.integrations.Services;
using Ariana_Mcp.Mcp;
using ModelContextProtocol.Server;

namespace Ariana_Mcp.Mcp.Tools;

[McpServerToolType]
public sealed class OperationsTools(OperationsService operationsService)
{
    [McpServerTool(
        Name = "search_orders",
        Title = "Search internal orders",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Searches internal orders from sample intake. Use when the user wants to know which order created a sample or which orders are open or active.")]
    public Task<string> SearchOrders(
        [Description("Advanced search for orders. Example: search by requester, status, or sample description when ArianaLab search criteria are known.")]
        string? q = null,
        [Description("Maximum number of matches (1-100, default 25). Example: 25.")]
        int limit = 25,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => operationsService.SearchOrdersAsync(q, limit, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "get_order",
        Title = "Load internal order",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description("Loads an internal order from sample intake. Use when a specific order ID is known.")]
    public Task<string> GetOrder(
        [Description("Order ID from ArianaLab, for example an ObjectId from a search result.")]
        string id,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => operationsService.GetOrderAsync(id, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "search_customer_orders",
        Title = "Search customer orders",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Searches customer orders that originate from customers or import profiles. Use when the user asks for a customer order number, EO number, or imported request.")]
    public Task<string> SearchCustomerOrders(
        [Description("Advanced search for customer orders. Example: search by customer order number, EO number, requester, or status.")]
        string? q = null,
        [Description("Maximum number of matches (1-100, default 25). Example: 25.")]
        int limit = 25,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => operationsService.SearchCustomerOrdersAsync(q, limit, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "get_customer_order",
        Title = "Load customer order",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description("Loads a customer order. Use when a specific customer order ID from ArianaLab is known.")]
    public Task<string> GetCustomerOrder(
        [Description("Customer order ID from ArianaLab, for example an ObjectId from a search result.")]
        string id,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => operationsService.GetCustomerOrderAsync(id, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "get_planning_orders",
        Title = "Search planning data",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Searches planning or order data in a specified module. Use when the user asks generally about planning, sample intake, or customer orders and the module can be specified.")]
    public Task<string> GetPlanningOrders(
        [Description("Planning module: 'orders' for internal orders or 'customer-orders' for customer orders. German aliases 'auftraege' and 'kundenauftraege' are also accepted.")]
        string module,
        [Description("Advanced search for the selected module when ArianaLab search criteria are known.")]
        string? q = null,
        [Description("Maximum number of matches (1-100, default 25). Example: 25.")]
        int limit = 25,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => operationsService.GetPlanningOrdersAsync(module, q, limit, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "get_system_info",
        Title = "Check ArianaLab connection",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Checks whether ArianaLab is reachable and which user the MCP is connected as. Use for diagnosis when tools do not work or permissions are unclear.")]
    public Task<string> GetSystemInfo(
        [Description("When true, also loads user permissions/claims. Use only for permission questions.")]
        bool includeClaims = false,
        [Description("When true, also runs KeepAlive. Use only when server diagnostics are explicitly requested.")]
        bool includeKeepAlive = false,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => operationsService.GetSystemInfoAsync(includeClaims, includeKeepAlive, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "search_cor",
        Title = "Search COR orders",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Searches Customer Order Requests (COR). Use only when the user explicitly asks for COR, customer orders, execution orders, or gateway orders; may contain invoice and payment data.")]
    public Task<string> SearchCor(
        [Description("Advanced search for COR orders. Example: search criteria for customer, status, or order number.")]
        string? q = null,
        [Description("Must be true because COR orders may contain sensitive customer, order, invoice, or payment data.")]
        bool includeSensitiveOrderData = false,
        [Description("Maximum number of matches (1-100, default 25). Example: 25.")]
        int limit = 25,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => operationsService.SearchCorAsync(q, includeSensitiveOrderData, limit, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "get_cor",
        Title = "Load COR order",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Loads a Customer Order Request (COR). Use only when the user explicitly asks for a specific COR order; may contain invoice and payment data.")]
    public Task<string> GetCor(
        [Description("COR ID, for example from a search result.")]
        string corId,
        [Description("Must be true because a COR order may contain sensitive customer, order, invoice, or payment data.")]
        bool includeSensitiveOrderData = false,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => operationsService.GetCorAsync(corId, includeSensitiveOrderData, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "validate_cor_gateway",
        Title = "Validate COR gateway order",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Checks a COR gateway order for business plausibility without saving it. Use only when the user explicitly wants COR validation.")]
    public Task<string> ValidateCorGateway(
        [Description("COR gateway record as JSON. May contain customer, order, invoice, or payment data.")]
        string dtoJson,
        [Description("Must be true because the COR record may contain sensitive customer, order, invoice, or payment data.")]
        bool includeSensitiveOrderData = false,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => operationsService.ValidateCorGatewayAsync(dtoJson, includeSensitiveOrderData, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "search_invoices",
        Title = "Search invoices",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Searches invoices. Use only when the user explicitly asks for an invoice, invoice number, billing, or open invoice data; contains sensitive billing data.")]
    public Task<string> SearchInvoices(
        [Description("Advanced search for invoices. Example: search criteria for invoice number, customer, status, or lab journal number.")]
        string? q = null,
        [Description("Must be true because invoices contain sensitive billing data.")]
        bool includeSensitiveBillingData = false,
        [Description("Maximum number of matches (1-100, default 25). Example: 25.")]
        int limit = 25,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => operationsService.SearchInvoicesAsync(q, includeSensitiveBillingData, limit, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "get_invoice",
        Title = "Load invoice",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Loads an invoice. Use only when the user explicitly needs invoice data; contains sensitive billing data.")]
    public Task<string> GetInvoice(
        [Description("Invoice number, for example '2026-000123'.")]
        string id,
        [Description("Must be true because an invoice contains sensitive billing data.")]
        bool includeSensitiveBillingData = false,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => operationsService.GetInvoiceAsync(id, includeSensitiveBillingData, cancellationToken),
            cancellationToken);
}
