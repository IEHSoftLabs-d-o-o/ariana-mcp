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
        "Sucht Kunden anhand eines Teilnamens oder einer Teil-ID und gibt eine kompakte Trefferliste (Kunden-ID und Name) zurück. " +
        "Dieses Tool zuerst verwenden, wenn nur ein Teil des Kundennamens bekannt ist.")]
    public Task<string> SearchCustomers(
        [Description("Teil des Kundennamens oder der Kunden-ID (mindestens 2 Zeichen).")]
        string search,
        [Description("Maximale Anzahl der Treffer (1-50, Standard 25).")]
        int limit = 25,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => customerService.SearchCustomersAsync(search, limit, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "customer_by_name",
        Title = "Kunde nach exaktem Namen",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Sucht einen Kunden anhand des exakten Namens und gibt das vollständige Kunden-JSON aus ArianaLab zurück. " +
        "Erfordert einen exakt passenden Namen; bei Teilnamen bitte search_customers verwenden.")]
    public Task<string> CustomerByName(
        [Description("Exakter Kundenname wie in ArianaLab gespeichert.")]
        string name,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => customerService.GetCustomerByNameAsync(name, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "customer_info_by_id",
        Title = "Kundeninformationen nach ID",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description(
        "Gibt detaillierte Kundeninformationen (Kontaktdaten, Adressen, Abrechnungsdaten) zu einer bekannten numerischen Kunden-ID zurück. " +
        "Bei nur bekanntem Namen zuerst search_customers oder customer_by_name verwenden.")]
    public Task<string> CustomerInfoById(
        [Description("Numerische ArianaLab-Kunden-ID (KundeId), z. B. '14197'.")]
        string customerId,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => customerService.GetCustomerInfoAsync(customerId, cancellationToken),
            cancellationToken);
}
