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
        Title = "Kunden suchen",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Sucht Kunden nach Name oder Kundennummer. Verwenden, wenn der Nutzer einen Kunden nur ungefähr benennt oder die Kundennummer nicht kennt.")]
    public Task<string> SearchCustomers(
        [Description("Erweiterte EasyQuery-Suche als JSON. Optional, wenn 'name' angegeben ist.")]
        string? q = null,
        [Description("Teil des Kundennamens oder der Kundennummer, z. B. 'Müller' oder '14197'. Mindestens 2 Zeichen.")]
        string? name = null,
        [Description("Maximale Trefferanzahl (1-50, Standard 25).")]
        int limit = 25,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => customerService.SearchCustomersAsync(q, name, limit, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "search_customers_batch",
        Title = "Kunden Batch-Suche",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Sucht Kunden anhand mehrerer Suchbegriffe und gibt pro Begriff eine kompakte Trefferliste zurück. " +
        "Fehlende Einträge brechen die gesamte Anfrage nicht ab.")]
    public Task<string> SearchCustomersBatch(
        [Description("Liste von Teilnamen oder Teil-Kundennummern, z. B. ['Müller', '1419'].")]
        IReadOnlyList<string> searches,
        [Description("Maximale Treffer pro Suchbegriff (1-50, Standard 25).")]
        int limit = 25,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => customerService.SearchCustomersBatchAsync(searches, limit, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "customer_by_name",
        Title = "Kunde nach exaktem Namen",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Sucht einen oder mehrere Kunden anhand exakter Namen und gibt pro Name das vollständige Kunden-JSON zurück. " +
        "Erfordert exakt passende Namen; bei Teilnamen bitte search_customers verwenden.")]
    public Task<string> CustomerByName(
        [Description("Liste exakter Kundennamen, z. B. ['Firma GmbH', 'Labor AG'].")]
        IReadOnlyList<string> names,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => customerService.GetCustomersByNamesAsync(names, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "customer_info_by_id",
        Title = "Kundeninformationen nach ID",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Gibt detaillierte Kundeninformationen zu einer oder mehreren Kundennummern zurück. " +
        "Kann Adressen, Ansprechpartner und weitere personenbezogene Daten enthalten.")]
    public Task<string> CustomerInfoById(
        [Description("Liste von Kundennummern, z. B. ['14197', '14198'].")]
        IReadOnlyList<string> customerIds,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => customerService.GetCustomerInfosAsync(customerIds, cancellationToken),
            cancellationToken);
}
