using System.Text.Json;
using Ariana_Mcp.integrations.Helpers;

namespace Ariana_Mcp.integrations.Services;

internal static class ArianaLabJsonHelpers
{
    public static IReadOnlyList<JsonElement> ExtractCollectionItems(string json)
    {
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        if (root.ValueKind == JsonValueKind.Array)
            return root.EnumerateArray().Select(item => item.Clone()).ToList();

        if (root.ValueKind != JsonValueKind.Object)
            return [];

        if (!TryGetProperty(root, "_embedded", out var embedded))
            return [];

        if (embedded.ValueKind == JsonValueKind.Array)
            return embedded.EnumerateArray().Select(item => item.Clone()).ToList();

        if (embedded.ValueKind == JsonValueKind.Object)
        {
            var items = new List<JsonElement>();
            foreach (var property in embedded.EnumerateObject())
            {
                if (property.Value.ValueKind == JsonValueKind.Array)
                    items.AddRange(property.Value.EnumerateArray().Select(item => item.Clone()));
                else
                    items.Add(property.Value.Clone());
            }

            return items;
        }

        return [];
    }

    public static string CreateCompactCollectionJson(
        string json,
        int limit,
        params string[] preferredFields)
    {
        var items = ExtractCollectionItems(json);
        var summaries = items
            .Take(Math.Clamp(limit, 1, 200))
            .Select(item => CompactObject(item, preferredFields))
            .Where(summary => summary.Count > 0)
            .ToList();

        return ArianaLabJson.Serialize(new
        {
            totalGefunden = items.Count,
            zurueckgegeben = summaries.Count,
            eintraege = summaries,
        });
    }

    public static Dictionary<string, object?> CompactObject(JsonElement element, params string[] preferredFields)
    {
        var result = new Dictionary<string, object?>(StringComparer.Ordinal);

        foreach (var field in preferredFields)
        {
            if (!TryGetProperty(element, field, out var value))
                continue;

            AddIfPresent(result, field, ToPlainValue(value));
        }

        if (result.Count > 0)
            return result;

        if (element.ValueKind != JsonValueKind.Object)
        {
            var plainValue = ToPlainValue(element);
            return JsonResponseCleaner.IsEmptyValue(plainValue)
                ? []
                : new Dictionary<string, object?> { ["wert"] = plainValue };
        }

        foreach (var property in element.EnumerateObject().Take(12))
        {
            if (property.Name is "_links" or "_embedded")
                continue;

            AddIfPresent(result, property.Name, ToPlainValue(property.Value));
        }

        return result;
    }

    private static void AddIfPresent(Dictionary<string, object?> target, string key, object? value)
    {
        if (!JsonResponseCleaner.IsEmptyValue(value))
            target[key] = value;
    }

    public static string? TryGetString(JsonElement element, params string[] propertyNames)
    {
        foreach (var propertyName in propertyNames)
        {
            if (!TryGetProperty(element, propertyName, out var value))
                continue;

            if (value.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
                continue;

            var text = value.ValueKind == JsonValueKind.String ? value.GetString() : value.ToString();
            if (!string.IsNullOrWhiteSpace(text))
                return text;
        }

        return null;
    }

    private static bool TryGetProperty(JsonElement element, string propertyName, out JsonElement value)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in element.EnumerateObject())
            {
                if (string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase))
                {
                    value = property.Value;
                    return true;
                }
            }
        }

        value = default;
        return false;
    }

    private static object? CleanArrayValue(JsonElement value)
    {
        var items = value.EnumerateArray()
            .Select(ToPlainValue)
            .Where(item => !JsonResponseCleaner.IsEmptyValue(item))
            .Take(20)
            .ToList();

        return items.Count == 0 ? null : items;
    }

    private static object? ToPlainValue(JsonElement value) =>
        value.ValueKind switch
        {
            JsonValueKind.String => string.IsNullOrWhiteSpace(value.GetString()) ? null : value.GetString(),
            JsonValueKind.Number when value.TryGetInt64(out var longValue) => longValue,
            JsonValueKind.Number when value.TryGetDecimal(out var decimalValue) => decimalValue,
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null or JsonValueKind.Undefined => null,
            JsonValueKind.Array => CleanArrayValue(value),
            JsonValueKind.Object => CleanObjectValue(value),
            _ => value.ToString(),
        };

    private static object? CleanObjectValue(JsonElement value)
    {
        var properties = value.EnumerateObject()
            .Where(property => property.Name is not "_links" and not "_embedded")
            .Take(12)
            .Select(property => new KeyValuePair<string, object?>(property.Name, ToPlainValue(property.Value)))
            .Where(pair => !JsonResponseCleaner.IsEmptyValue(pair.Value))
            .ToDictionary(pair => pair.Key, pair => pair.Value);

        return properties.Count == 0 ? null : properties;
    }
}
