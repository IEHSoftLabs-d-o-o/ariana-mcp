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
            throw new ArianaLabException("name must not be empty.");

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
                $"No customer found with exact name '{name}'. Try search_customers with a partial name.",
                HttpStatusCode.NotFound,
                ex);
        }

        var kundeId = TryGetKundeId(customerInfoBody);
        if (kundeId is null)
        {
            throw new ArianaLabException(
                $"No customer found with exact name '{name}'. Try search_customers with a partial name.");
        }

        var customerUri = $"Rest/Mad/Kunden/{Uri.EscapeDataString(kundeId)}";
        try
        {
            return await GetAsStringAsync(client, customerUri, cancellationToken);
        }
        catch (ArianaLabException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new ArianaLabException(
                $"Customer '{name}' was found (id {kundeId}) but full customer data is unavailable.",
                HttpStatusCode.NotFound,
                ex);
        }
    }

    public async Task<string> GetCustomerInfoAsync(
        string customerId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(customerId))
            throw new ArianaLabException("customerId must not be empty.");

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
                $"No customer information found for customer id '{customerId}'.",
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
            throw new ArianaLabException("search must not be empty.");

        var trimmedSearch = search.Trim();
        if (trimmedSearch.Length < 2)
            throw new ArianaLabException("search must be at least 2 characters.");

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
            throw new ArianaLabException("Unexpected customer list format from ArianaLab.");

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
