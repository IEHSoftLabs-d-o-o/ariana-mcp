using System.Net;
using Ariana_Mcp.integrations.Exceptions;

namespace Ariana_Mcp.integrations.Services;

public sealed class OperationsService(IHttpClientFactory httpClientFactory)
    : ArianaLabServiceBase(httpClientFactory)
{
    private const int DefaultLimit = 25;
    private const int MaxLimit = 100;

    public Task<string> SearchOrdersAsync(string? q, int limit = DefaultLimit, CancellationToken cancellationToken = default) =>
        SearchHalAsync("Rest/Opd/Probenanlage/Auftraege", q, limit, cancellationToken);

    public Task<string> GetOrderAsync(string id, CancellationToken cancellationToken = default) =>
        GetRequiredAsync(
            $"Rest/Opd/Probenanlage/Auftraege/{Uri.EscapeDataString(RequireText(id, "Die Auftrags-ID darf nicht leer sein."))}",
            $"Kein interner Auftrag mit der ID '{id}' gefunden.",
            cancellationToken);

    public Task<string> SearchCustomerOrdersAsync(string? q, int limit = DefaultLimit, CancellationToken cancellationToken = default) =>
        SearchHalAsync("Rest/Opd/Probenanlage/Kundenauftraege", q, limit, cancellationToken);

    public Task<string> GetCustomerOrderAsync(string id, CancellationToken cancellationToken = default) =>
        GetRequiredAsync(
            $"Rest/Opd/Probenanlage/Kundenauftraege/{Uri.EscapeDataString(RequireText(id, "Die Kundenauftrags-ID darf nicht leer sein."))}",
            $"Kein Kundenauftrag mit der ID '{id}' gefunden.",
            cancellationToken);

    public Task<string> GetPlanningOrdersAsync(
        string module,
        string? q,
        int limit = DefaultLimit,
        CancellationToken cancellationToken = default)
    {
        var route = module.Trim().ToLowerInvariant() switch
        {
            "auftraege" or "aufträge" or "orders" or "interne-auftraege" or "interne-aufträge" =>
                "Rest/Opd/Probenanlage/Auftraege",
            "kundenauftraege" or "kundenaufträge" or "customer-orders" or "kunden" =>
                "Rest/Opd/Probenanlage/Kundenauftraege",
            _ => throw new ArianaLabException(
                "Unbekanntes Planungsmodul. Bitte 'auftraege' oder 'kundenauftraege' verwenden."),
        };

        return SearchHalAsync(route, q, limit, cancellationToken);
    }

    public async Task<string> GetSystemInfoAsync(
        bool includeClaims = false,
        bool includeKeepAlive = false,
        CancellationToken cancellationToken = default)
    {
        var client = CreateClient();
        var currentUser = await GetAsStringAsync(client, "Rest/Sys/Info/currentUser", cancellationToken).ConfigureAwait(false);
        var user = await GetAsStringAsync(client, "Rest/User", cancellationToken).ConfigureAwait(false);
        string? claims = null;
        string? keepAlive = null;

        if (includeClaims)
            claims = await GetAsStringAsync(client, "Rest/User/Claims", cancellationToken).ConfigureAwait(false);

        if (includeKeepAlive)
            keepAlive = await GetAsStringAsync(client, "Rest/Sys/KeepAlive", cancellationToken).ConfigureAwait(false);

        return ArianaLabJson.Serialize(new
        {
            erreichbar = true,
            aktuellerBenutzerInfo = TryParseJson(currentUser),
            aktuellerBenutzer = TryParseJson(user),
            claims = claims is null ? null : TryParseJson(claims),
            keepAlive = keepAlive is null ? null : TryParseJson(keepAlive),
            hinweis = includeKeepAlive
                ? "KeepAlive wurde ausdrücklich ausgeführt."
                : "KeepAlive wurde nicht ausgeführt, weil es auf dem ArianaLab-Server zusätzliche Arbeit auslösen kann.",
        });
    }

    public Task<string> SearchCorAsync(
        string? q,
        bool includeSensitiveOrderData,
        int limit = DefaultLimit,
        CancellationToken cancellationToken = default)
    {
        EnsureSensitive(includeSensitiveOrderData, "COR-Aufträge können Kunden-, Auftrags-, Rechnungs- und Zahlungsdaten enthalten.");
        return SearchHalAsync("Rest/Cors/CustomerOrderRequests", q, limit, cancellationToken);
    }

    public Task<string> GetCorAsync(
        string corId,
        bool includeSensitiveOrderData,
        CancellationToken cancellationToken = default)
    {
        EnsureSensitive(includeSensitiveOrderData, "Ein COR-Auftrag kann Kunden-, Auftrags-, Rechnungs- und Zahlungsdaten enthalten.");
        return GetRequiredAsync(
            $"Rest/Cors/CustomerOrderRequests/{Uri.EscapeDataString(RequireText(corId, "Die COR-ID darf nicht leer sein."))}",
            $"Kein COR-Auftrag mit der ID '{corId}' gefunden.",
            cancellationToken);
    }

    public Task<string> ValidateCorGatewayAsync(
        string dtoJson,
        bool includeSensitiveOrderData,
        CancellationToken cancellationToken = default)
    {
        EnsureSensitive(includeSensitiveOrderData, "Eine COR-Validierung kann Kunden-, Auftrags-, Rechnungs- und Zahlungsdaten enthalten.");

        if (string.IsNullOrWhiteSpace(dtoJson))
            throw new ArianaLabException("Der COR-Datensatz zur Prüfung darf nicht leer sein.");

        var client = CreateClient();
        return PostJsonAsStringAsync(client, "Rest/Cors/CustomerOrderRequestsGateway/validate", dtoJson, cancellationToken);
    }

    public Task<string> SearchInvoicesAsync(
        string? q,
        bool includeSensitiveBillingData,
        int limit = DefaultLimit,
        CancellationToken cancellationToken = default)
    {
        EnsureSensitive(includeSensitiveBillingData, "Rechnungen enthalten sensible Abrechnungsdaten.");
        return SearchHalAsync("Rest/Opd/Rechnungserstellung/Rechnungen", q, limit, cancellationToken);
    }

    public Task<string> GetInvoiceAsync(
        string id,
        bool includeSensitiveBillingData,
        CancellationToken cancellationToken = default)
    {
        EnsureSensitive(includeSensitiveBillingData, "Eine Rechnung enthält sensible Abrechnungsdaten.");
        return GetRequiredAsync(
            $"Rest/Opd/Rechnungserstellung/Rechnungen/{Uri.EscapeDataString(RequireText(id, "Die Rechnungsnummer darf nicht leer sein."))}",
            $"Keine Rechnung mit der Nummer '{id}' gefunden.",
            cancellationToken);
    }

    public Task<string> GetPlanningResourceAsync(
        string module,
        string id,
        CancellationToken cancellationToken = default)
    {
        var route = module.Trim().ToLowerInvariant() switch
        {
            "auftraege" or "aufträge" or "orders" => "Rest/Opd/Probenanlage/Auftraege",
            "kundenauftraege" or "kundenaufträge" or "customer-orders" => "Rest/Opd/Probenanlage/Kundenauftraege",
            _ => throw new ArianaLabException(
                "Unbekanntes Planungsmodul. Bitte 'auftraege' oder 'kundenauftraege' verwenden."),
        };

        return GetRequiredAsync(
            $"{route}/{Uri.EscapeDataString(RequireText(id, "Die Planungs-ID darf nicht leer sein."))}",
            $"Kein Planungsdatensatz im Modul '{module}' mit der ID '{id}' gefunden.",
            cancellationToken);
    }

    private async Task<string> SearchHalAsync(
        string route,
        string? q,
        int limit,
        CancellationToken cancellationToken)
    {
        limit = Math.Clamp(limit, 1, MaxLimit);
        var client = CreateClient();
        var json = await GetAsStringAsync(client, WithQuery(route, ("q", q)), cancellationToken).ConfigureAwait(false);
        return ArianaLabJsonHelpers.CreateCompactCollectionJson(json, limit);
    }

    private async Task<string> GetRequiredAsync(
        string requestUri,
        string notFoundMessage,
        CancellationToken cancellationToken)
    {
        var client = CreateClient();
        try
        {
            return await GetAsStringAsync(client, requestUri, cancellationToken).ConfigureAwait(false);
        }
        catch (ArianaLabException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new ArianaLabException(notFoundMessage, HttpStatusCode.NotFound, ex);
        }
    }

    private static void EnsureSensitive(bool confirmed, string reason)
    {
        if (!confirmed)
        {
            throw new ArianaLabException(
                $"{reason} Bitte dieses Tool nur verwenden, wenn der Nutzer ausdrücklich danach gefragt hat, und den Parameter zur Bestätigung auf true setzen.");
        }
    }

    private static string RequireText(string? value, string message)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArianaLabException(message);

        return value.Trim();
    }

    private static object? TryParseJson(string json)
    {
        try
        {
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            return doc.RootElement.Clone();
        }
        catch (System.Text.Json.JsonException)
        {
            return json;
        }
    }
}
