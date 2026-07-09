using System.Net;
using System.Text.Json;
using Ariana_Mcp.integrations.Exceptions;

namespace Ariana_Mcp.integrations.Services;

public sealed class CustomerService(IHttpClientFactory httpClientFactory)
    : ArianaLabServiceBase(httpClientFactory)
{
    private const int DefaultSearchLimit = 25;
    private const int MaxSearchLimit = 50;

    public async Task<string> GetCustomerByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArianaLabException("name darf nicht leer sein.");

        var client = CreateClient();
        var customerInfoUri =
            $"Rest/Mad/Kunden/KundenInformationen/ByName?name={Uri.EscapeDataString(name)}";

        string customerInfoBody;
        try
        {
            customerInfoBody = await GetAsStringAsync(client, customerInfoUri, cancellationToken);
        }
        catch (ArianaLabException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new ArianaLabException(
                $"Kein Kunde mit dem exakten Namen '{name}' gefunden. Bitte search_customers mit einem Teilnamen verwenden.",
                HttpStatusCode.NotFound,
                ex);
        }

        var kundeId = TryGetKundeId(customerInfoBody);
        if (kundeId is null)
        {
            throw new ArianaLabException(
                $"Kein Kunde mit dem exakten Namen '{name}' gefunden. Bitte search_customers mit einem Teilnamen verwenden.");
        }

        var customerUri = $"Rest/Mad/Kunden/{Uri.EscapeDataString(kundeId)}";
        try
        {
            return await GetAsStringAsync(client, customerUri, cancellationToken);
        }
        catch (ArianaLabException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new ArianaLabException(
                $"Kunde '{name}' wurde gefunden (ID {kundeId}), aber die vollständigen Kundendaten sind nicht verfügbar.",
                HttpStatusCode.NotFound,
                ex);
        }
    }

    public async Task<string> GetCustomerInfoAsync(
        string customerId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(customerId))
            throw new ArianaLabException("customerId darf nicht leer sein.");

        var client = CreateClient();
        var requestUri =
            $"Rest/Mad/Kunden/{Uri.EscapeDataString(customerId)}/KundenInformationen";

        try
        {
            return await GetAsStringAsync(client, requestUri, cancellationToken);
        }
        catch (ArianaLabException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new ArianaLabException(
                $"Keine Kundeninformationen für die Kunden-ID '{customerId}' gefunden.",
                HttpStatusCode.NotFound,
                ex);
        }
    }

    public async Task<string> SearchCustomersAsync(
        string search,
        int limit = DefaultSearchLimit,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(search))
            throw new ArianaLabException("search darf nicht leer sein.");

        var trimmedSearch = search.Trim();
        if (trimmedSearch.Length < 2)
            throw new ArianaLabException("search muss mindestens 2 Zeichen lang sein.");

        limit = Math.Clamp(limit, 1, MaxSearchLimit);

        var client = CreateClient();
        var allCustomersJson = await GetAsStringAsync(client, "Rest/Mad/Kunden", cancellationToken);
        var matches = FilterCustomers(allCustomersJson, trimmedSearch, limit);

        return JsonSerializer.Serialize(new
        {
            search = trimmedSearch,
            matchCount = matches.Count,
            limit,
            customers = matches,
        });
    }

    private static List<CustomerSummary> FilterCustomers(string json, string search, int limit)
    {
        using var doc = JsonDocument.Parse(json);
        if (doc.RootElement.ValueKind != JsonValueKind.Array)
            throw new ArianaLabException("Unerwartetes Format der Kundenliste von ArianaLab.");

        var matches = new List<CustomerSummary>();
        var comparison = StringComparison.OrdinalIgnoreCase;

        foreach (var element in doc.RootElement.EnumerateArray())
        {
            var summary = TryGetCustomerSummary(element);
            if (summary is null)
                continue;

            if (!summary.Name.Contains(search, comparison)
                && !summary.CustomerId.Contains(search, comparison))
            {
                continue;
            }

            matches.Add(summary);
            if (matches.Count >= limit)
                break;
        }

        return matches;
    }

    private static CustomerSummary? TryGetCustomerSummary(JsonElement element)
    {
        var customerId = TryGetStringProperty(element, "KundeId", "Id", "KundenId");
        var name = TryGetStringProperty(element, "Name", "KundenName", "Bezeichnung", "Firma");

        if (customerId is null && name is null)
            return null;

        return new CustomerSummary(customerId ?? string.Empty, name ?? string.Empty);
    }

    private static string? TryGetStringProperty(JsonElement element, params string[] propertyNames)
    {
        foreach (var propertyName in propertyNames)
        {
            if (!element.TryGetProperty(propertyName, out var value))
                continue;

            if (value.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
                continue;

            var text = value.ValueKind == JsonValueKind.String
                ? value.GetString()
                : value.ToString();

            if (!string.IsNullOrWhiteSpace(text))
                return text;
        }

        return null;
    }

    private static string? TryGetKundeId(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return null;

        try
        {
            using var doc = JsonDocument.Parse(json);
            if (!doc.RootElement.TryGetProperty("KundeId", out var kundeId))
                return null;

            if (kundeId.ValueKind == JsonValueKind.Null)
                return null;

            var value = kundeId.ValueKind == JsonValueKind.String
                ? kundeId.GetString()
                : kundeId.ToString();

            return string.IsNullOrWhiteSpace(value) ? null : value;
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private sealed record CustomerSummary(string CustomerId, string Name);
}
