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
        "Sucht Kunden anhand eines oder mehrerer Teilnamen bzw. Teil-IDs und gibt pro Suchbegriff eine kompakte Trefferliste (Kunden-ID und Name) zurück. " +
        "Fehlende oder ungültige Suchbegriffe führen nicht zum Abbruch der gesamten Anfrage. " +
        "Dieses Tool zuerst verwenden, wenn nur ein Teil des Kundennamens bekannt ist.")]
    public Task<string> SearchCustomers(
        [Description("Liste von Teilnamen oder Teil-Kunden-IDs (jeweils mindestens 2 Zeichen), z. B. ['Müller', '1419'].")]
        IReadOnlyList<string> searches,
        [Description("Maximale Anzahl der Treffer pro Suchbegriff (1-50, Standard 25).")]
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
        "Sucht einen oder mehrere Kunden anhand exakter Namen und gibt pro Name das vollständige Kunden-JSON aus ArianaLab zurück. " +
        "Fehlende oder ungültige Namen führen nicht zum Abbruch der gesamten Anfrage. " +
        "Erfordert exakt passende Namen; bei Teilnamen bitte search_customers verwenden.")]
    public Task<string> CustomerByName(
        [Description("Liste exakter Kundennamen wie in ArianaLab gespeichert, z. B. ['Firma GmbH', 'Labor AG'].")]
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
        "Gibt detaillierte Kundeninformationen (Kontaktdaten, Adressen, Abrechnungsdaten) zu einer oder mehreren bekannten numerischen Kunden-IDs zurück. " +
        "Fehlende oder ungültige IDs führen nicht zum Abbruch der gesamten Anfrage. " +
        "Bei nur bekanntem Namen zuerst search_customers oder customer_by_name verwenden.")]
    public Task<string> CustomerInfoById(
        [Description("Liste numerischer ArianaLab-Kunden-IDs (KundeId), z. B. ['14197', '14198'].")]
        IReadOnlyList<string> customerIds,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => customerService.GetCustomerInfosAsync(customerIds, cancellationToken),
            cancellationToken);
}
