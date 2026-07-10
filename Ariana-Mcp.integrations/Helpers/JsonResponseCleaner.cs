using System.Text.Json;
using Ariana_Mcp.integrations.Services;

namespace Ariana_Mcp.integrations.Helpers;

public static class JsonResponseCleaner
{
    public static string Clean(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return json;

        try
        {
            using var doc = JsonDocument.Parse(json);
            var cleaned = CleanElement(doc.RootElement);
            return cleaned switch
            {
                null => "{}",
                _ => JsonSerializer.Serialize(cleaned, ArianaLabJson.SerializerOptions),
            };
        }
        catch (JsonException)
        {
            return json;
        }
    }

    public static bool IsEmptyValue(object? value) =>
        value switch
        {
            null => true,
            string text => string.IsNullOrWhiteSpace(text),
            JsonElement element => IsEmptyElement(element),
            System.Collections.ICollection collection => collection.Count == 0,
            _ => false,
        };

    private static bool IsEmptyElement(JsonElement element) =>
        element.ValueKind switch
        {
            JsonValueKind.Null or JsonValueKind.Undefined => true,
            JsonValueKind.String => string.IsNullOrWhiteSpace(element.GetString()),
            JsonValueKind.Array => element.GetArrayLength() == 0,
            JsonValueKind.Object => !element.EnumerateObject().Any(),
            _ => false,
        };

    private static object? CleanElement(JsonElement element) =>
        element.ValueKind switch
        {
            JsonValueKind.Null or JsonValueKind.Undefined => null,
            JsonValueKind.String => CleanString(element.GetString()),
            JsonValueKind.Number when element.TryGetInt64(out var longValue) => longValue,
            JsonValueKind.Number when element.TryGetDecimal(out var decimalValue) => decimalValue,
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Array => CleanArray(element),
            JsonValueKind.Object => CleanObject(element),
            _ => element.ToString(),
        };

    private static object? CleanString(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value;

    private static object? CleanArray(JsonElement element)
    {
        var items = new List<object?>();
        foreach (var item in element.EnumerateArray())
        {
            var cleaned = CleanElement(item);
            if (cleaned is not null)
                items.Add(cleaned);
        }

        return items.Count == 0 ? null : items;
    }

    private static object? CleanObject(JsonElement element)
    {
        var result = new Dictionary<string, object?>(StringComparer.Ordinal);
        foreach (var property in element.EnumerateObject())
        {
            var cleaned = CleanElement(property.Value);
            if (cleaned is not null)
                result[property.Name] = cleaned;
        }

        return result.Count == 0 ? null : result;
    }
}
