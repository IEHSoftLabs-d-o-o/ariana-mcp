using Ariana_Mcp.integrations.Exceptions;
using Ariana_Mcp.integrations.Helpers;

namespace Ariana_Mcp.integrations.Services;

public sealed class OrderService(IHttpClientFactory httpClientFactory)
    : ArianaLabServiceBase(httpClientFactory)
{
    private const int DefaultSearchLimit = 25;
    private const int MaxSearchLimit = 50;

    public async Task<string> SearchOrdersAsync(
        string? q,
        int limit = DefaultSearchLimit,
        CancellationToken cancellationToken = default)
    {
        limit = Math.Clamp(limit, 1, MaxSearchLimit);
        var query = string.IsNullOrWhiteSpace(q)
            ? EasyQueryBuilder.BuildJson(limit: limit)
            : EasyQueryBuilder.EnsureLimit(q, limit);
        var body = await GetQueryAsync("Rest/Opd/Probenanlage/Auftraege", query, cancellationToken);
        return HalResponseHelper.EnsureEmbeddedOrReturnRaw(body);
    }

    public async Task<string> GetOrderAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArianaLabException("id darf nicht leer sein.");

        return await GetAsync($"Rest/Opd/Probenanlage/Auftraege/{Uri.EscapeDataString(id)}", cancellationToken);
    }

    public async Task<string> SearchCustomerOrdersAsync(
        string? q,
        int limit = DefaultSearchLimit,
        CancellationToken cancellationToken = default)
    {
        limit = Math.Clamp(limit, 1, MaxSearchLimit);
        var query = string.IsNullOrWhiteSpace(q)
            ? EasyQueryBuilder.BuildJson(limit: limit)
            : EasyQueryBuilder.EnsureLimit(q, limit);
        var body = await GetQueryAsync("Rest/Opd/Probenanlage/Kundenauftraege", query, cancellationToken);
        return HalResponseHelper.EnsureEmbeddedOrReturnRaw(body);
    }

    public async Task<string> SearchSchreibstellenAsync(
        string? journalNumber = null,
        string? client = null,
        string? invoiceRecipient = null,
        string? customerSampleNumber = null,
        string? sampleDescription = null,
        string? reportedCompleteFrom = null,
        string? reportedCompleteTo = null,
        string? reportedCompleteBy = null,
        string? category = null,
        string? testCategory = null,
        string? additionalFieldName = null,
        string? additionalFieldContent = null,
        bool? labeled = true,
        bool? done = null,
        bool? reportedComplete = false,
        bool? archived = false,
        bool? release = null,
        bool? urgent = null,
        IReadOnlyList<string>? packages = null,
        string? customerGroup1 = null,
        IReadOnlyList<string>? sampleGroups = null,
        string? productGroup = null,
        string? sampleReceiptFrom = null,
        string? sampleReceiptTo = null,
        string? deadlineFrom = null,
        string? deadlineTo = null,
        string? taggedOnFrom = null,
        string? taggedOnTo = null,
        string? taggedBy = null,
        IReadOnlyList<string>? departments = null,
        string? sequenceTemplate = null,
        bool? matchingSequenceTemplate = null,
        int limit = DefaultSearchLimit,
        CancellationToken cancellationToken = default)
    {
        limit = Math.Clamp(limit, 1, MaxSearchLimit);
        var conditions = new List<EasyQueryCondition>();

        AddContains(conditions, "tagebuchnummer", journalNumber);
        AddContains(conditions, "auftraggeber", client);
        AddContains(conditions, "rechnungsempfaenger", invoiceRecipient);
        AddContains(conditions, "kundenprobennummer", customerSampleNumber);
        AddContains(conditions, "probenbezeichnung", sampleDescription);
        AddDateRange(conditions, "fertiggemeldetam", reportedCompleteFrom, reportedCompleteTo);
        AddContains(conditions, "fertiggemeldetvon", reportedCompleteBy);
        AddContains(conditions, "kategorie", category);
        AddContains(conditions, "pruefkategorie", testCategory);
        AddAdditionalField(conditions, additionalFieldName, additionalFieldContent);
        AddBoolean(conditions, "etikettiert", labeled);
        AddBoolean(conditions, "erledigt", done);
        AddBoolean(conditions, "fertiggemeldet", reportedComplete);
        AddBoolean(conditions, "archiviert", archived);
        AddBoolean(conditions, "freigabe", release);
        AddBoolean(conditions, "eilig", urgent);
        AddIdentifierEquals(conditions, "pruefpaket", packages);
        AddContains(conditions, "kundengruppe1", customerGroup1);
        AddAny(conditions, "probengruppe", sampleGroups);
        AddContains(conditions, "produktgruppe", productGroup);
        AddDateRange(conditions, "probeneingang", sampleReceiptFrom, sampleReceiptTo);
        AddDateRange(conditions, "termin", deadlineFrom, deadlineTo);
        AddDateRange(conditions, "etikettiertam", taggedOnFrom, taggedOnTo);
        AddContains(conditions, "etikettiertvon", taggedBy);
        AddIdentifierEquals(conditions, "abteilung", departments);
        AddEquals(conditions, "sequence_template", sequenceTemplate);
        if (matchingSequenceTemplate is true)
            AddEquals(conditions, "matching_sequence", "1");

        var query = EasyQueryBuilder.BuildJson(conditions, limit);
        var body = await GetQueryAsync("Rest/Opd/Schreibstelle", query, cancellationToken);
        return HalResponseHelper.EnsureEmbeddedOrReturnRaw(body);
    }

    public async Task<string> StartSequenceAsync(
        IReadOnlyList<SchreibstelleSequenceStart> sequences,
        CancellationToken cancellationToken = default)
    {
        if (sequences is null || sequences.Count == 0)
            throw new ArianaLabException("Mindestens eine Probe muss für die Sequenz ausgewählt werden.");

        if (sequences.Any(sequence => string.IsNullOrWhiteSpace(sequence.Name)))
            throw new ArianaLabException("Jede zu startende Sequenz muss einen Sequenznamen enthalten.");

        if (sequences.Any(sequence => string.IsNullOrWhiteSpace(sequence.Reference)))
            throw new ArianaLabException("Jede zu startende Sequenz muss eine Tagebuchnummer enthalten.");

        var body = sequences.Select(sequence => new SchreibstelleSequenceStartRequest
        {
            Type = "SchreibstelleProbeSequence",
            Name = sequence.Name.Trim(),
            Reference = sequence.Reference.Trim(),
        }).ToList();

        await PostAsStringAsync("Rest/Opd/Sequence/Schreibstelle/StartSequence", body, cancellationToken)
            .ConfigureAwait(false);

        return ArianaLabJson.Serialize(new
        {
            message = "Sequence start completed successfully.",
            started = body.Select(sequence => new { sequence.Name, sequence.Reference }),
        });
    }

    public async Task<string> GetCustomerOrderAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArianaLabException("id darf nicht leer sein.");

        return await GetAsync(
            $"Rest/Opd/Probenanlage/Kundenauftraege/{Uri.EscapeDataString(id)}",
            cancellationToken);
    }

    public async Task<string> GetPlanningOrdersAsync(
        string module,
        string? q,
        int limit = DefaultSearchLimit,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(module))
            throw new ArianaLabException("module darf nicht leer sein.");

        return module.ToLowerInvariant() switch
        {
            "auftraege" or "probenanlage" or "intern" => await SearchOrdersAsync(q, limit, cancellationToken),
            "kundenauftraege" or "kundenauftrag" => await SearchCustomerOrdersAsync(q, limit, cancellationToken),
            _ => throw new ArianaLabException(
                $"Unbekanntes Planungsmodul '{module}'. Erlaubt: auftraege, kundenauftraege."),
        };
    }

    public async Task<string> GetPlanningOrderAsync(
        string module,
        string id,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(module))
            throw new ArianaLabException("module darf nicht leer sein.");

        return module.ToLowerInvariant() switch
        {
            "auftraege" or "probenanlage" or "intern" => await GetOrderAsync(id, cancellationToken),
            "kundenauftraege" or "kundenauftrag" => await GetCustomerOrderAsync(id, cancellationToken),
            _ => throw new ArianaLabException(
                $"Unbekanntes Planungsmodul '{module}'. Erlaubt: auftraege, kundenauftraege."),
        };
    }

    private static void AddContains(List<EasyQueryCondition> conditions, string property, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
            conditions.Add(EasyQueryCondition.Contains(property, value.Trim()));
    }

    private static void AddEquals(List<EasyQueryCondition> conditions, string property, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
            conditions.Add(EasyQueryCondition.Equals(property, value.Trim()));
    }

    private static void AddBoolean(List<EasyQueryCondition> conditions, string property, bool? value)
    {
        if (value.HasValue)
            conditions.Add(EasyQueryCondition.Equals(property, value.Value ? "1" : "0"));
    }

    private static void AddDateRange(
        List<EasyQueryCondition> conditions,
        string property,
        string? from,
        string? to)
    {
        if (!string.IsNullOrWhiteSpace(from))
            conditions.Add(EasyQueryCondition.GreaterOrEqual($"{property}von", from.Trim()));

        if (!string.IsNullOrWhiteSpace(to))
            conditions.Add(EasyQueryCondition.LessOrEqual($"{property}bis", to.Trim()));
    }

    private static void AddAdditionalField(
        List<EasyQueryCondition> conditions,
        string? fieldName,
        string? content)
    {
        if (string.IsNullOrWhiteSpace(fieldName) || string.IsNullOrWhiteSpace(content))
            return;

        conditions.Add(EasyQueryCondition.Contains(
            $"zusatzfeld[{fieldName.Trim()}]",
            content.Trim()));
    }

    private static void AddIdentifierEquals(
        List<EasyQueryCondition> conditions,
        string property,
        IReadOnlyList<string>? identifiers)
    {
        var values = identifiers?
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select(value => value.Trim())
            .ToArray();

        if (values is not { Length: > 0 })
            return;

        conditions.Add(EasyQueryCondition.Equals(
            $"{property}[{string.Join("','", values)}]",
            "1"));
    }

    private static void AddAny(
        List<EasyQueryCondition> conditions,
        string property,
        IReadOnlyList<string>? values)
    {
        var patterns = values?
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select(value => value.Trim())
            .ToArray();

        if (patterns is { Length: > 0 })
            conditions.Add(new EasyQueryCondition(property, "()", patterns));
    }
}

public sealed class SchreibstelleSequenceStart
{
    public string Name { get; init; } = string.Empty;
    public string Reference { get; init; } = string.Empty;
}

internal sealed class SchreibstelleSequenceStartRequest
{
    [System.Text.Json.Serialization.JsonPropertyName("_t")]
    public string Type { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Reference { get; init; } = string.Empty;
}
