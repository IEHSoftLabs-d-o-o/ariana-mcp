using System.Text.Json;

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

            result[field] = ToPlainValue(value);
        }

        if (result.Count > 0)
            return result;

        if (element.ValueKind != JsonValueKind.Object)
            return new Dictionary<string, object?> { ["wert"] = ToPlainValue(element) };

        foreach (var property in element.EnumerateObject().Take(12))
        {
            if (property.Name is "_links" or "_embedded")
                continue;

            result[property.Name] = ToPlainValue(property.Value);
        }

        return result;
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

    private static object? ToPlainValue(JsonElement value) =>
        value.ValueKind switch
        {
            JsonValueKind.String => value.GetString(),
            JsonValueKind.Number when value.TryGetInt64(out var longValue) => longValue,
            JsonValueKind.Number when value.TryGetDecimal(out var decimalValue) => decimalValue,
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null or JsonValueKind.Undefined => null,
            JsonValueKind.Array => value.EnumerateArray().Take(20).Select(ToPlainValue).ToList(),
            JsonValueKind.Object => value.EnumerateObject()
                .Where(property => property.Name is not "_links" and not "_embedded")
                .Take(12)
                .ToDictionary(property => property.Name, property => ToPlainValue(property.Value)),
            _ => value.ToString(),
        };
}
