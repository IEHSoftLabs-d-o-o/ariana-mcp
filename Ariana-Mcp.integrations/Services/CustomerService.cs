using System.Net;
using System.Text.Json;
using Ariana_Mcp.integrations.Exceptions;
using Ariana_Mcp.integrations.Helpers;

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

        var customerInfoUri =
            $"Rest/Mad/Kunden/KundenInformationen/ByName?name={Uri.EscapeDataString(name)}";

        string customerInfoBody;
        try
        {
            customerInfoBody = await GetAsync(customerInfoUri, cancellationToken);
        }
        catch (ArianaLabException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new ArianaLabException(
                $"No customer with the exact name '{name}' was found. Use search_customers with a partial name.",
                HttpStatusCode.NotFound,
                ex);
        }

        var kundeId = TryGetKundeId(customerInfoBody);
        if (kundeId is null)
        {
            throw new ArianaLabException(
                $"No customer with the exact name '{name}' was found. Use search_customers with a partial name.");
        }

        return await GetCustomerAsync(kundeId, cancellationToken);
    }

    public Task<string> GetCustomersByNamesAsync(
        IReadOnlyList<string> names,
        CancellationToken cancellationToken = default)
        => BatchLookupHelper.ExecuteAsync(
            names,
            "names",
            "name darf nicht leer sein.",
            "name",
            GetCustomerByNameAsync,
            cancellationToken);

    public async Task<string> GetCustomerAsync(
        string nummer,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(nummer))
            throw new ArianaLabException("nummer darf nicht leer sein.");

        var requestUri = $"Rest/Mad/Kunden/{Uri.EscapeDataString(nummer)}";
        try
        {
            return await GetAsync(requestUri, cancellationToken);
        }
        catch (ArianaLabException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new ArianaLabException(
                $"Kein Kunde mit der Nummer '{nummer}' gefunden.",
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

        var requestUri =
            $"Rest/Mad/Kunden/{Uri.EscapeDataString(customerId)}/KundenInformationen";

        try
        {
            return await GetAsync(requestUri, cancellationToken);
        }
        catch (ArianaLabException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new ArianaLabException(
                $"Keine Kundeninformationen für die Kundennummer '{customerId}' gefunden.",
                HttpStatusCode.NotFound,
                ex);
        }
    }

    public Task<string> GetCustomerInfosAsync(
        IReadOnlyList<string> customerIds,
        CancellationToken cancellationToken = default)
        => BatchLookupHelper.ExecuteAsync(
            customerIds,
            "customerId darf nicht leer sein.",
            GetCustomerInfoAsync,
            cancellationToken);

    public async Task<string> GetCustomerInfoBySampleAsync(
        string tagebuchnummer,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(tagebuchnummer))
            throw new ArianaLabException("tagebuchnummer darf nicht leer sein.");

        var encoded = ArianaLabUriHelper.EncodePathSegment(tagebuchnummer);
        var requestUri =
            $"Rest/Mad/Kunden/KundenInformationen/ByProbe?tagebuchnummer={Uri.EscapeDataString(encoded)}";

        try
        {
            return await GetAsync(requestUri, cancellationToken);
        }
        catch (ArianaLabException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new ArianaLabException(
                $"Keine Kundeninformationen zur Probe '{tagebuchnummer}' gefunden.",
                HttpStatusCode.NotFound,
                ex);
        }
    }

    public async Task<string> SearchCustomersAsync(
        string? q,
        string? name,
        int limit = DefaultSearchLimit,
        CancellationToken cancellationToken = default)
    {
        var query = BuildCustomerSearchQuery(q, name);
        if (string.IsNullOrWhiteSpace(query))
            throw new ArianaLabException("Mindestens einer der Parameter 'q' oder 'name' muss angegeben werden.");

        limit = Math.Clamp(limit, 1, MaxSearchLimit);

        var effectiveQuery = query.TrimStart().StartsWith('{')
            ? EasyQueryBuilder.EnsureLimit(query, limit)
            : EasyQueryBuilder.BuildJson(ParseQueryConditions(query), limit);

        var body = await GetQueryAsync("Rest/Mad/Kunden", effectiveQuery, cancellationToken);
        return HalResponseHelper.ProjectCustomers(body, limit);
    }

    public Task<string> SearchCustomersBatchAsync(
        IReadOnlyList<string> searches,
        int limit = DefaultSearchLimit,
        CancellationToken cancellationToken = default)
    {
        limit = Math.Clamp(limit, 1, MaxSearchLimit);

        return BatchLookupHelper.ExecuteAsync(
            searches,
            "searches",
            "search darf nicht leer sein.",
            "search",
            (search, ct) => SearchCustomersAsync(null, search, limit, ct),
            cancellationToken,
            envelopeExtras: new Dictionary<string, object?> { ["limit"] = limit });
    }

    private static string? BuildCustomerSearchQuery(string? q, string? name)
    {
        if (!string.IsNullOrWhiteSpace(q))
            return q.Trim();

        if (string.IsNullOrWhiteSpace(name))
            return null;

        var trimmed = name.Trim();
        if (trimmed.Length < 2)
            throw new ArianaLabException("name muss mindestens 2 Zeichen lang sein.");

        return EasyQueryBuilder.BuildContains("Anzeigename", trimmed);
    }

    private static IReadOnlyList<EasyQueryCondition> ParseQueryConditions(string query)
    {
        if (query.TrimStart().StartsWith('{'))
        {
            using var doc = JsonDocument.Parse(query);
            if (!doc.RootElement.TryGetProperty("Conditions", out var conditions)
                || conditions.ValueKind != JsonValueKind.Array)
            {
                return [];
            }

            var result = new List<EasyQueryCondition>();
            foreach (var condition in conditions.EnumerateArray())
            {
                var property = HalResponseHelper.TryGetStringProperty(condition, "Property");
                var op = HalResponseHelper.TryGetStringProperty(condition, "Operator") ?? "=";
                var pattern = condition.TryGetProperty("Pattern", out var patternElement)
                    ? patternElement.ValueKind == JsonValueKind.String
                        ? patternElement.GetString() ?? string.Empty
                        : patternElement.ToString()
                    : string.Empty;

                if (!string.IsNullOrWhiteSpace(property))
                    result.Add(new EasyQueryCondition(property, op, pattern));
            }

            return result;
        }

        return query.Split('|', StringSplitOptions.RemoveEmptyEntries)
            .Select(part =>
            {
                var eq = part.IndexOf('=');
                if (eq <= 0)
                    return new EasyQueryCondition(part, "~*", $"*{part}*");

                var property = part[..eq];
                var pattern = part[(eq + 1)..];
                return property.Contains('~')
                    ? new EasyQueryCondition(property[..property.IndexOf('~')], property[property.IndexOf('~')..], pattern)
                    : EasyQueryCondition.Contains(property, pattern);
            })
            .ToList();
    }

    private static string? TryGetKundeId(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return null;

        try
        {
            using var doc = JsonDocument.Parse(json);
            return HalResponseHelper.TryGetStringProperty(doc.RootElement, "KundeId", "Nummer", "Id");
        }
        catch (JsonException)
        {
            return null;
        }
    }
}
