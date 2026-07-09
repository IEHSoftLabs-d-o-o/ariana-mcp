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
        "Searches customers by partial name or id and returns a compact list of matches (customer id and name). " +
        "Use this first when you only know part of a customer name.")]
    public Task<string> SearchCustomers(
        [Description("Partial customer name or id to search for (minimum 2 characters).")]
        string search,
        [Description("Maximum number of matches to return (1-50, default 25).")]
        int limit = 25,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => customerService.SearchCustomersAsync(search, limit, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "customer_by_name",
        Title = "Customer by exact name",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Looks up a customer by exact name and returns full customer JSON from ArianaLab. " +
        "Requires an exact name match; prefer search_customers for partial names.")]
    public Task<string> CustomerByName(
        [Description("Exact customer name as stored in ArianaLab.")]
        string name,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => customerService.GetCustomerByNameAsync(name, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "customer_info_by_id",
        Title = "Customer information by id",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Returns detailed customer information (contact data, addresses, billing details) for a known numeric customer id. " +
        "Use search_customers or customer_by_name first if you only have a name.")]
    public Task<string> CustomerInfoById(
        [Description("Numeric ArianaLab customer id (KundeId), e.g. '14197'.")]
        string customerId,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => customerService.GetCustomerInfoAsync(customerId, cancellationToken),
            cancellationToken);
}
