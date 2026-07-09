using System.Text.Json;

namespace Ariana_Mcp.integrations.Helpers;

public static class EasyQueryBuilder
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = null,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
    };

    public static string BuildJson(
        IReadOnlyList<EasyQueryCondition>? conditions = null,
        int? limit = null,
        int pageNumber = 0)
    {
        var query = new Dictionary<string, object?>
        {
            ["Conditions"] = conditions?.Select(ToDictionary).ToList() ?? [],
            ["Sortings"] = Array.Empty<object>(),
            ["ResultFields"] = Array.Empty<object>(),
            ["Paging"] = limit is > 0
                ? new Dictionary<string, object> { ["PageNumber"] = pageNumber, ["PageSize"] = limit.Value }
                : null,
        };

        return JsonSerializer.Serialize(query, SerializerOptions);
    }

    public static string BuildContains(string property, string value, int? limit = null) =>
        BuildJson([EasyQueryCondition.Contains(property, value)], limit);

    public static string BuildEquals(string property, string value, int? limit = null) =>
        BuildJson([EasyQueryCondition.Equals(property, value)], limit);

    public static string BuildFromSimplifiedFilters(
        IReadOnlyDictionary<string, string?> filters,
        int? limit = null)
    {
        var conditions = new List<EasyQueryCondition>();

        foreach (var (property, value) in filters)
        {
            if (string.IsNullOrWhiteSpace(value))
                continue;

            conditions.Add(EasyQueryCondition.Contains(property, value.Trim()));
        }

        return BuildJson(conditions, limit);
    }

    public static string AppendQueryParameter(string requestUri, string? q)
    {
        if (string.IsNullOrWhiteSpace(q))
            return requestUri;

        var separator = requestUri.Contains('?') ? '&' : '?';
        return $"{requestUri}{separator}q={Uri.EscapeDataString(q)}";
    }

    public static string EnsureLimit(string? queryJson, int limit, int pageNumber = 0)
    {
        if (string.IsNullOrWhiteSpace(queryJson) || !queryJson.TrimStart().StartsWith('{'))
            return BuildJson(limit: limit, pageNumber: pageNumber);

        using var doc = JsonDocument.Parse(queryJson);
        var conditions = new List<EasyQueryCondition>();

        if (doc.RootElement.TryGetProperty("Conditions", out var conditionsElement)
            && conditionsElement.ValueKind == JsonValueKind.Array)
        {
            foreach (var condition in conditionsElement.EnumerateArray())
            {
                var property = condition.TryGetProperty("Property", out var propertyElement)
                    ? propertyElement.GetString()
                    : null;
                var op = condition.TryGetProperty("Operator", out var opElement)
                    ? opElement.GetString() ?? "="
                    : "=";
                object pattern = condition.TryGetProperty("Pattern", out var patternElement)
                    ? patternElement.ValueKind switch
                    {
                        JsonValueKind.String => patternElement.GetString() ?? string.Empty,
                        JsonValueKind.Number => patternElement.GetDouble(),
                        JsonValueKind.True => true,
                        JsonValueKind.False => false,
                        _ => patternElement.ToString(),
                    }
                    : string.Empty;

                if (!string.IsNullOrWhiteSpace(property))
                    conditions.Add(new EasyQueryCondition(property, op, pattern));
            }
        }

        return BuildJson(conditions, limit, pageNumber);
    }

    private static Dictionary<string, object?> ToDictionary(EasyQueryCondition condition) => new()
    {
        ["Property"] = condition.Property,
        ["Operator"] = condition.Operator,
        ["Pattern"] = condition.Pattern,
    };
}

public readonly record struct EasyQueryCondition(string Property, string Operator, object Pattern)
{
    public static EasyQueryCondition Contains(string property, string value) =>
        new(property, "~*", $"*{value}*");

    public static EasyQueryCondition Equals(string property, string value) =>
        new(property, "=", value);

    public static EasyQueryCondition GreaterOrEqual(string property, string value) =>
        new(property, ">=", value);

    public static EasyQueryCondition LessOrEqual(string property, string value) =>
        new(property, "<=", value);
}
