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

    [McpServerTool(Name = "search_schreibstellen", Title = "Search Schreibstellen", ReadOnly = true, Idempotent = true)]
    [Description(
        "Searches Schreibstellen from ArianaLab using explicit search-mask fields. Leave fields empty when they should not filter results. For yes/no fields, null means no filter.")]
    public Task<string> SearchSchreibstellen(
        [Description("Journal number / Tagebuchnummer, for example 26-0812074.")]
        string? journalNumber = null,
        [Description("Client / Auftraggeber name or number.")]
        string? client = null,
        [Description("Invoice recipient / Rechnungsempfänger.")]
        string? invoiceRecipient = null,
        [Description("Customer sample number / Kundenprobennummer.")]
        string? customerSampleNumber = null,
        [Description("Sample description / Probenbezeichnung.")]
        string? sampleDescription = null,
        [Description("Reported-complete date from, format yyyy-MM-dd.")]
        string? reportedCompleteFrom = null,
        [Description("Reported-complete date to, format yyyy-MM-dd.")]
        string? reportedCompleteTo = null,
        [Description("User who reported the sample complete.")]
        string? reportedCompleteBy = null,
        [Description("Category / Kategorie.")]
        string? category = null,
        [Description("Test category / Prüfkategorie.")]
        string? testCategory = null,
        [Description("Additional field name. Use together with additionalFieldContent.")]
        string? additionalFieldName = null,
        [Description("Additional field content. Use together with additionalFieldName.")]
        string? additionalFieldContent = null,
        [Description("Labeled / etikettiert: true for yes, false for no, null for no filter.")]
        bool? labeled = true,
        [Description("Done / erledigt: true for yes, false for no, null for no filter.")]
        bool? done = null,
        [Description("Reported complete / fertiggemeldet: true for yes, false for no, null for no filter.")]
        bool? reportedComplete = false,
        [Description("Archived / archiviert: true for yes, false for no, null for no filter.")]
        bool? archived = false,
        [Description("Release / Freigabe: true for yes, false for no, null for no filter.")]
        bool? release = null,
        [Description("Urgent / eilig: true for yes, false for no, null for no filter.")]
        bool? urgent = null,
        [Description("Test packages / Prüfpakete, for example ['Mikrobiologie','Chemie'].")]
        IReadOnlyList<string>? packages = null,
        [Description("Customer group 1 / Kundengruppe 1.")]
        string? customerGroup1 = null,
        [Description("Sample groups / Probengruppen, for example ['Planproben'].")]
        IReadOnlyList<string>? sampleGroups = null,
        [Description("Product group / Produktgruppe.")]
        string? productGroup = null,
        [Description("Sample receipt date from, format yyyy-MM-dd.")]
        string? sampleReceiptFrom = null,
        [Description("Sample receipt date to, format yyyy-MM-dd.")]
        string? sampleReceiptTo = null,
        [Description("Deadline from, format yyyy-MM-dd.")]
        string? deadlineFrom = null,
        [Description("Deadline to, format yyyy-MM-dd.")]
        string? deadlineTo = null,
        [Description("Labeled date from, format yyyy-MM-dd.")]
        string? taggedOnFrom = null,
        [Description("Labeled date to, format yyyy-MM-dd.")]
        string? taggedOnTo = null,
        [Description("User who labeled the sample.")]
        string? taggedBy = null,
        [Description("Departments / Abteilungen to require, for example ['Mibi'] or ['Chemie','Mibi'].")]
        IReadOnlyList<string>? departments = null,
        [Description("Sequence template name. Use the exact ArianaLab template name.")]
        string? sequenceTemplate = null,
        [Description("Matching sequence template: true restricts to samples with a matching template; null means no filter. False is treated as no filter because ArianaLab does not expose a separate non-matching query.")]
        bool? matchingSequenceTemplate = null,
        [Description("Maximum number of matches (1-50, default 25).")]
        int limit = 25,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => orderService.SearchSchreibstellenAsync(
                journalNumber,
                client,
                invoiceRecipient,
                customerSampleNumber,
                sampleDescription,
                reportedCompleteFrom,
                reportedCompleteTo,
                reportedCompleteBy,
                category,
                testCategory,
                additionalFieldName,
                additionalFieldContent,
                labeled,
                done,
                reportedComplete,
                archived,
                release,
                urgent,
                packages,
                customerGroup1,
                sampleGroups,
                productGroup,
                sampleReceiptFrom,
                sampleReceiptTo,
                deadlineFrom,
                deadlineTo,
                taggedOnFrom,
                taggedOnTo,
                taggedBy,
                departments,
                sequenceTemplate,
                matchingSequenceTemplate,
                limit,
                cancellationToken),
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

    [McpServerTool(
        Name = "start_sequence",
        Title = "Start Schreibstelle sequence",
        ReadOnly = false,
        Idempotent = false,
        Destructive = false)]
    [Description(
        "Starts Schreibstelle sequences in ArianaLab for the supplied samples and sequence names. " +
        "This changes ArianaLab data and must only be used after the user explicitly confirmed the start.")]
    public Task<string> StartSequence(
        [Description("Sequences to start. Every item needs the configured sequence name and the sample's lab journal number (reference).")]
        IReadOnlyList<SchreibstelleSequenceStart> sequences,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => orderService.StartSequenceAsync(sequences, cancellationToken),
            cancellationToken);
}
