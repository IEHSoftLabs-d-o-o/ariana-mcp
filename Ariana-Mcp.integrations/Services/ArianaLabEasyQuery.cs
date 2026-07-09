using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ariana_Mcp.integrations.Services;

internal sealed class ArianaLabEasyQuery
{
    private readonly List<EasyCondition> _conditions = [];
    private readonly List<EasySorting> _sortings = [];

    public bool HasConditions => _conditions.Count > 0;

    public ArianaLabEasyQuery AddEquals(string property, object? value) =>
        AddCondition(property, "=", value);

    public ArianaLabEasyQuery AddGreaterThanOrEqual(string property, object? value) =>
        AddCondition(property, ">=", value);

    public ArianaLabEasyQuery AddLessThanOrEqual(string property, object? value) =>
        AddCondition(property, "<=", value);

    public ArianaLabEasyQuery AddSimpleMatch(string property, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return this;

        var pattern = value.Trim();
        if (!pattern.Contains('*'))
            pattern = $"*{pattern}*";

        return AddCondition(property, "~*", pattern);
    }

    public ArianaLabEasyQuery AddSorting(string property, bool descending = false)
    {
        if (!string.IsNullOrWhiteSpace(property))
            _sortings.Add(new EasySorting(property.Trim(), descending ? "desc" : "asc"));

        return this;
    }

    public ArianaLabEasyQuery AddCondition(string property, string op, object? value)
    {
        if (string.IsNullOrWhiteSpace(property) || string.IsNullOrWhiteSpace(op) || IsEmpty(value))
            return this;

        _conditions.Add(new EasyCondition(property.Trim(), op, value!));
        return this;
    }

    public string ToJson() =>
        JsonSerializer.Serialize(
            new EasyQueryDto(_conditions, _sortings, Paging: null, ResultFields: []),
            ArianaLabJson.SerializerOptions);

    private static bool IsEmpty(object? value) =>
        value is null || value is string text && string.IsNullOrWhiteSpace(text);

    private sealed record EasyQueryDto(
        IReadOnlyList<EasyCondition> Conditions,
        IReadOnlyList<EasySorting> Sortings,
        object? Paging,
        IReadOnlyList<object> ResultFields);

    private sealed record EasyCondition(
        string Property,
        [property: JsonPropertyName("Operator")] string OperatorValue,
        object Pattern);

    private sealed record EasySorting(string Property, string Direction);
}
