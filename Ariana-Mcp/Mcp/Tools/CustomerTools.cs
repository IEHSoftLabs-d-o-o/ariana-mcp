using System.ComponentModel;
using Ariana_Mcp.integrations.Services;
using Ariana_Mcp.Mcp;
using ModelContextProtocol.Server;

namespace Ariana_Mcp.Mcp.Tools;

[McpServerToolType]
public sealed class CustomerTools(CustomerService customerService)
{
    [McpServerTool(
        Name = "search_customers",
        Title = "Search customers",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Searches customers by name or customer number. Use when the user names a customer approximately or does not know the customer number.")]
    public Task<string> SearchCustomers(
        [Description("Advanced EasyQuery search as JSON. Optional when 'name' is provided.")]
        string? q = null,
        [Description("Part of the customer name or customer number, for example 'Müller' or '14197'. At least 2 characters.")]
        string? name = null,
        [Description("Maximum number of matches (1-50, default 25).")]
        int limit = 25,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => customerService.SearchCustomersAsync(q, name, limit, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "search_customers_batch",
        Title = "Batch customer search",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Searches customers by multiple search terms and returns a compact match list for each term. " +
        "Missing entries do not fail the entire request.")]
    public Task<string> SearchCustomersBatch(
        [Description("List of partial names or partial customer numbers, for example ['Müller', '1419'].")]
        IReadOnlyList<string> searches,
        [Description("Maximum matches per search term (1-50, default 25).")]
        int limit = 25,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => customerService.SearchCustomersBatchAsync(searches, limit, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "customer_by_name",
        Title = "Customer by exact name",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Searches for one or more customers by exact names and returns the full customer JSON for each name. " +
        "Requires exact matching names; use search_customers for partial names.")]
    public Task<string> CustomerByName(
        [Description("List of exact customer names, for example ['Firma GmbH', 'Labor AG'].")]
        IReadOnlyList<string> names,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => customerService.GetCustomersByNamesAsync(names, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "customer_info_by_id",
        Title = "Customer information by ID",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Returns detailed customer information for one or more customer numbers. " +
        "May contain addresses, contacts, and other personal data.")]
    public Task<string> CustomerInfoById(
        [Description("List of customer numbers, for example ['14197', '14198'].")]
        IReadOnlyList<string> customerIds,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => customerService.GetCustomerInfosAsync(customerIds, cancellationToken),
            cancellationToken);
}
