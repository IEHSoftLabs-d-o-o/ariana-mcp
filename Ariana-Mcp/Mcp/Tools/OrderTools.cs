using System.ComponentModel;
using Ariana_Mcp.integrations.Services;
using Ariana_Mcp.Mcp;
using ModelContextProtocol.Server;

namespace Ariana_Mcp.Mcp.Tools;

[McpServerToolType]
public sealed class OrderTools(OrderService orderService)
{
    [McpServerTool(Name = "search_orders", Title = "Search internal orders", ReadOnly = true, Idempotent = true)]
    [Description(
        "Searches internal orders from sample intake. Use when the user wants to know which order created a sample or which orders are open or active.")]
    public Task<string> SearchOrders(
        [Description("Advanced EasyQuery search as JSON. Optional.")]
        string? q = null,
        [Description("Maximum number of matches (1-50, default 25).")]
        int limit = 25,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => orderService.SearchOrdersAsync(q, limit, cancellationToken),
            cancellationToken);

    [McpServerTool(Name = "get_order", Title = "Load internal order", ReadOnly = true, Idempotent = true)]
    [Description("Loads an internal order from sample intake by order ID.")]
    public Task<string> GetOrder(
        [Description("Order ID from sample intake.")]
        string id,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => orderService.GetOrderAsync(id, cancellationToken),
            cancellationToken);

    [McpServerTool(Name = "search_customer_orders", Title = "Search customer orders", ReadOnly = true, Idempotent = true)]
    [Description(
        "Searches customer orders that originate from customers or import profiles. Use for customer order numbers, EO numbers, or imported requests.")]
    public Task<string> SearchCustomerOrders(
        [Description("Advanced EasyQuery search as JSON. Optional.")]
        string? q = null,
        [Description("Maximum number of matches (1-50, default 25).")]
        int limit = 25,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => orderService.SearchCustomerOrdersAsync(q, limit, cancellationToken),
            cancellationToken);

    [McpServerTool(Name = "get_customer_order", Title = "Load customer order", ReadOnly = true, Idempotent = true)]
    [Description("Loads a customer order by ID.")]
    public Task<string> GetCustomerOrder(
        [Description("Customer order ID.")]
        string id,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => orderService.GetCustomerOrderAsync(id, cancellationToken),
            cancellationToken);

    [McpServerTool(Name = "get_planning_orders", Title = "Search planning orders", ReadOnly = true, Idempotent = true)]
    [Description(
        "Searches planning or order data in a specified module. Use for general questions about planning, sample intake, or customer orders.")]
    public Task<string> GetPlanningOrders(
        [Description("Module: 'orders' or 'customer-orders'. German aliases 'auftraege' and 'kundenauftraege' are also accepted.")]
        string module,
        [Description("Advanced EasyQuery search as JSON. Optional.")]
        string? q = null,
        [Description("Maximum number of matches (1-50, default 25).")]
        int limit = 25,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => orderService.GetPlanningOrdersAsync(module, q, limit, cancellationToken),
            cancellationToken);
}
