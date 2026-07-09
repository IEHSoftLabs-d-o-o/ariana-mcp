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
        Title = "Interne Aufträge suchen",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Sucht interne Aufträge aus der Probenanlage. Verwenden, wenn der Nutzer wissen möchte, aus welchem Auftrag eine Probe entstanden ist oder welche Aufträge offen/aktiviert sind.")]
    public Task<string> SearchOrders(
        [Description("Erweiterte Suche für Aufträge. Beispiel: Suche nach Auftraggeber, Status oder Probenbezeichnung, wenn ArianaLab-Suchkriterien bekannt sind.")]
        string? q = null,
        [Description("Maximale Anzahl Treffer (1-100, Standard 25). Beispiel: 25.")]
        int limit = 25,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => operationsService.SearchOrdersAsync(q, limit, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "get_order",
        Title = "Internen Auftrag laden",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description("Lädt einen internen Auftrag aus der Probenanlage. Verwenden, wenn eine konkrete Auftrags-ID bekannt ist.")]
    public Task<string> GetOrder(
        [Description("Auftrags-ID aus ArianaLab, z. B. eine ObjectId aus einem Suchtreffer.")]
        string id,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => operationsService.GetOrderAsync(id, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "search_customer_orders",
        Title = "Kundenaufträge suchen",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Sucht Kundenaufträge, die von Kunden oder Importprofilen kommen. Verwenden, wenn der Nutzer nach einer Kundenauftragsnummer, EO-Nummer oder importierten Beauftragung fragt.")]
    public Task<string> SearchCustomerOrders(
        [Description("Erweiterte Suche für Kundenaufträge. Beispiel: Suche nach Kundenauftragsnummer, EO-Nummer, Auftraggeber oder Status.")]
        string? q = null,
        [Description("Maximale Anzahl Treffer (1-100, Standard 25). Beispiel: 25.")]
        int limit = 25,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => operationsService.SearchCustomerOrdersAsync(q, limit, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "get_customer_order",
        Title = "Kundenauftrag laden",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description("Lädt einen Kundenauftrag. Verwenden, wenn eine konkrete Kundenauftrags-ID aus ArianaLab bekannt ist.")]
    public Task<string> GetCustomerOrder(
        [Description("Kundenauftrags-ID aus ArianaLab, z. B. eine ObjectId aus einem Suchtreffer.")]
        string id,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => operationsService.GetCustomerOrderAsync(id, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "get_planning_orders",
        Title = "Planungsdaten suchen",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Sucht Planungs- oder Auftragsdaten in einem angegebenen Modul. Verwenden, wenn der Nutzer allgemein nach Planung, Probenanlage oder Kundenaufträgen fragt und das Modul angegeben werden kann.")]
    public Task<string> GetPlanningOrders(
        [Description("Planungsmodul: 'auftraege' für interne Aufträge oder 'kundenauftraege' für Kundenaufträge. Beispiel: 'auftraege'.")]
        string module,
        [Description("Erweiterte Suche für das gewählte Modul, wenn ArianaLab-Suchkriterien bekannt sind.")]
        string? q = null,
        [Description("Maximale Anzahl Treffer (1-100, Standard 25). Beispiel: 25.")]
        int limit = 25,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => operationsService.GetPlanningOrdersAsync(module, q, limit, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "get_system_info",
        Title = "ArianaLab-Verbindung prüfen",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Prüft, ob ArianaLab erreichbar ist und mit welchem Benutzer der MCP verbunden ist. Verwenden zur Diagnose, wenn Tools nicht funktionieren oder Berechtigungen unklar sind.")]
    public Task<string> GetSystemInfo(
        [Description("Wenn true, werden zusätzlich Benutzerrechte/Claims geladen. Nur bei Berechtigungsfragen verwenden.")]
        bool includeClaims = false,
        [Description("Wenn true, wird zusätzlich KeepAlive ausgeführt. Nur verwenden, wenn ausdrücklich eine Serverdiagnose gewünscht ist.")]
        bool includeKeepAlive = false,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => operationsService.GetSystemInfoAsync(includeClaims, includeKeepAlive, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "search_cor",
        Title = "COR-Aufträge suchen",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Sucht Customer Order Requests (COR). Nur verwenden, wenn der Nutzer ausdrücklich nach COR, Kundenbestellung, Ausführungsauftrag oder Gateway-Auftrag fragt; kann Rechnungs- und Zahlungsdaten enthalten.")]
    public Task<string> SearchCor(
        [Description("Erweiterte Suche für COR-Aufträge. Beispiel: Suchkriterien zu Kunde, Status oder Auftragsnummer.")]
        string? q = null,
        [Description("Muss true sein, weil COR-Aufträge sensible Kunden-, Auftrags-, Rechnungs- oder Zahlungsdaten enthalten können.")]
        bool includeSensitiveOrderData = false,
        [Description("Maximale Anzahl Treffer (1-100, Standard 25). Beispiel: 25.")]
        int limit = 25,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => operationsService.SearchCorAsync(q, includeSensitiveOrderData, limit, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "get_cor",
        Title = "COR-Auftrag laden",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Lädt einen Customer Order Request (COR). Nur verwenden, wenn der Nutzer ausdrücklich nach einem konkreten COR-Auftrag fragt; kann Rechnungs- und Zahlungsdaten enthalten.")]
    public Task<string> GetCor(
        [Description("COR-ID, z. B. aus einem Suchtreffer.")]
        string corId,
        [Description("Muss true sein, weil ein COR-Auftrag sensible Kunden-, Auftrags-, Rechnungs- oder Zahlungsdaten enthalten kann.")]
        bool includeSensitiveOrderData = false,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => operationsService.GetCorAsync(corId, includeSensitiveOrderData, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "validate_cor_gateway",
        Title = "COR-Gateway-Auftrag prüfen",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Prüft einen COR-Gateway-Auftrag auf fachliche Plausibilität, ohne ihn zu speichern. Nur verwenden, wenn der Nutzer ausdrücklich eine COR-Validierung möchte.")]
    public Task<string> ValidateCorGateway(
        [Description("COR-Gateway-Datensatz als JSON. Kann Kunden-, Auftrags-, Rechnungs- oder Zahlungsdaten enthalten.")]
        string dtoJson,
        [Description("Muss true sein, weil der COR-Datensatz sensible Kunden-, Auftrags-, Rechnungs- oder Zahlungsdaten enthalten kann.")]
        bool includeSensitiveOrderData = false,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => operationsService.ValidateCorGatewayAsync(dtoJson, includeSensitiveOrderData, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "search_invoices",
        Title = "Rechnungen suchen",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Sucht Rechnungen. Nur verwenden, wenn der Nutzer ausdrücklich nach Rechnung, Rechnungsnummer, Abrechnung oder offenen Rechnungsdaten fragt; enthält sensible Abrechnungsdaten.")]
    public Task<string> SearchInvoices(
        [Description("Erweiterte Suche für Rechnungen. Beispiel: Suchkriterien zu Rechnungsnummer, Kunde, Status oder Tagebuchnummer.")]
        string? q = null,
        [Description("Muss true sein, weil Rechnungen sensible Abrechnungsdaten enthalten.")]
        bool includeSensitiveBillingData = false,
        [Description("Maximale Anzahl Treffer (1-100, Standard 25). Beispiel: 25.")]
        int limit = 25,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => operationsService.SearchInvoicesAsync(q, includeSensitiveBillingData, limit, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "get_invoice",
        Title = "Rechnung laden",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Lädt eine Rechnung. Nur verwenden, wenn der Nutzer ausdrücklich Rechnungsdaten benötigt; enthält sensible Abrechnungsdaten.")]
    public Task<string> GetInvoice(
        [Description("Rechnungsnummer, z. B. '2026-000123'.")]
        string id,
        [Description("Muss true sein, weil eine Rechnung sensible Abrechnungsdaten enthält.")]
        bool includeSensitiveBillingData = false,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => operationsService.GetInvoiceAsync(id, includeSensitiveBillingData, cancellationToken),
            cancellationToken);
}
