using System.ComponentModel;
using Ariana_Mcp.integrations.Services;
using Ariana_Mcp.Mcp;
using ModelContextProtocol.Server;

namespace Ariana_Mcp.Mcp.Tools;

[McpServerToolType]
public sealed class OrderTools(OrderService orderService)
{
    [McpServerTool(Name = "search_orders", Title = "Interne Aufträge suchen", ReadOnly = true, Idempotent = true)]
    [Description(
        "Sucht interne Aufträge aus der Probenanlage. Verwenden, wenn der Nutzer wissen möchte, aus welchem Auftrag eine Probe entstanden ist oder welche Aufträge offen/aktiviert sind.")]
    public Task<string> SearchOrders(
        [Description("Erweiterte EasyQuery-Suche als JSON. Optional.")]
        string? q = null,
        [Description("Maximale Trefferanzahl (1-50, Standard 25).")]
        int limit = 25,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => orderService.SearchOrdersAsync(q, limit, cancellationToken),
            cancellationToken);

    [McpServerTool(Name = "get_order", Title = "Internen Auftrag laden", ReadOnly = true, Idempotent = true)]
    [Description("Lädt einen internen Auftrag aus der Probenanlage anhand der Auftrags-ID.")]
    public Task<string> GetOrder(
        [Description("Auftrags-ID aus der Probenanlage.")]
        string id,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => orderService.GetOrderAsync(id, cancellationToken),
            cancellationToken);

    [McpServerTool(Name = "search_customer_orders", Title = "Kundenaufträge suchen", ReadOnly = true, Idempotent = true)]
    [Description(
        "Sucht Kundenaufträge, die von Kunden oder Importprofilen kommen. Verwenden bei Kundenauftragsnummer, EO-Nummer oder importierter Beauftragung.")]
    public Task<string> SearchCustomerOrders(
        [Description("Erweiterte EasyQuery-Suche als JSON. Optional.")]
        string? q = null,
        [Description("Maximale Trefferanzahl (1-50, Standard 25).")]
        int limit = 25,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => orderService.SearchCustomerOrdersAsync(q, limit, cancellationToken),
            cancellationToken);

    [McpServerTool(Name = "get_customer_order", Title = "Kundenauftrag laden", ReadOnly = true, Idempotent = true)]
    [Description("Lädt einen Kundenauftrag anhand der ID.")]
    public Task<string> GetCustomerOrder(
        [Description("Kundenauftrags-ID.")]
        string id,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => orderService.GetCustomerOrderAsync(id, cancellationToken),
            cancellationToken);

    [McpServerTool(Name = "get_planning_orders", Title = "Planungsaufträge suchen", ReadOnly = true, Idempotent = true)]
    [Description(
        "Sucht Planungs- oder Auftragsdaten in einem angegebenen Modul. Verwenden bei allgemeinen Fragen zu Planung, Probenanlage oder Kundenaufträgen.")]
    public Task<string> GetPlanningOrders(
        [Description("Modul: 'auftraege' oder 'kundenauftraege'.")]
        string module,
        [Description("Erweiterte EasyQuery-Suche als JSON. Optional.")]
        string? q = null,
        [Description("Maximale Trefferanzahl (1-50, Standard 25).")]
        int limit = 25,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => orderService.GetPlanningOrdersAsync(module, q, limit, cancellationToken),
            cancellationToken);
}
