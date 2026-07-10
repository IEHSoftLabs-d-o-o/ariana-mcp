using System.Text.Json;
using Ariana_Mcp.integrations.Services;

namespace Ariana_Mcp.integrations.Helpers;

public static class HalResponseHelper
{
    public static List<JsonElement> ExtractEmbeddedItems(string json)
    {
        using var doc = JsonDocument.Parse(json);
        return ExtractEmbeddedItems(doc.RootElement);
    }

    public static List<JsonElement> ExtractEmbeddedItems(JsonElement root)
    {
        if (root.ValueKind == JsonValueKind.Array)
            return root.EnumerateArray().ToList();

        if (!root.TryGetProperty("_embedded", out var embedded))
            return [];

        return embedded.ValueKind switch
        {
            JsonValueKind.Array => embedded.EnumerateArray().ToList(),
            JsonValueKind.Object => embedded.EnumerateObject()
                .SelectMany(property => property.Value.ValueKind == JsonValueKind.Array
                    ? property.Value.EnumerateArray()
                    : [])
                .ToList(),
            _ => [],
        };
    }

    public static string ProjectCompact(
        string json,
        IReadOnlyList<string> fields,
        int? limit = null)
    {
        var items = ExtractEmbeddedItems(json);
        if (limit is > 0)
            items = items.Take(limit.Value).ToList();

        var projected = items
            .Select(item => ProjectItem(item, fields))
            .ToList();

        return ArianaLabJson.Serialize(new { count = projected.Count, items = projected });
    }

    public static string ProjectCustomers(string json, int? limit = null) =>
        ProjectCompact(json, ["Nummer", "Anzeigename"], limit);

    public static string? TryGetStringProperty(JsonElement element, params string[] propertyNames)
    {
        foreach (var propertyName in propertyNames)
        {
            if (!element.TryGetProperty(propertyName, out var value))
                continue;

            if (value.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
                continue;

            var text = value.ValueKind == JsonValueKind.String ? value.GetString() : value.ToString();
            if (!string.IsNullOrWhiteSpace(text))
                return text;
        }

        return null;
    }

    public static string EnsureEmbeddedOrReturnRaw(string json)
    {
        var items = ExtractEmbeddedItems(json);
        if (items.Count == 0 && !json.Contains("_embedded", StringComparison.Ordinal))
            return json;

        return ArianaLabJson.Serialize(new { count = items.Count, items });
    }

    private static Dictionary<string, string> ProjectItem(JsonElement element, IReadOnlyList<string> fields)
    {
        var result = new Dictionary<string, string>(fields.Count);
        foreach (var field in fields)
        {
            var value = TryGetStringProperty(element, field);
            if (!string.IsNullOrWhiteSpace(value))
                result[field] = value;
        }

        return result;
    }
}
