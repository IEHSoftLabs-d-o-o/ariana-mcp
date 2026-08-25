using System.ComponentModel;
using System.Net;
using System.Text.Json.Serialization;
using Ariana_Mcp.integrations.Exceptions;
using Ariana_Mcp.integrations.Helpers;

namespace Ariana_Mcp.integrations.Services;

public sealed class SampleService(IHttpClientFactory httpClientFactory)
    : ArianaLabServiceBase(httpClientFactory)
{
    private const int DefaultSearchLimit = 25;
    private const int MaxSearchLimit = 100;
    private static readonly HashSet<string> AdditionalFieldOperators = [">", ">=", "<", "<=", "=", "~*", "!="];

    public async Task<string> GetSampleByIdAsync(
        string tagebuchnummer,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(tagebuchnummer))
            throw new ArianaLabException("tagebuchnummer darf nicht leer sein.");

        var requestUri = ArianaLabUriHelper.BuildSamplePath(tagebuchnummer);

        try
        {
            return await GetAsync(requestUri, cancellationToken);
        }
        catch (ArianaLabException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new ArianaLabException(
                $"Keine Probe mit der Tagebuchnummer '{tagebuchnummer}' gefunden.",
                HttpStatusCode.NotFound,
                ex);
        }
    }

    public Task<string> GetSamplesByIdsAsync(
        IReadOnlyList<string> sampleIds,
        CancellationToken cancellationToken = default)
        => BatchLookupHelper.ExecuteAsync(
            sampleIds,
            "tagebuchnummer darf nicht leer sein.",
            GetSampleByIdAsync,
            cancellationToken);

    public async Task<string> GetSampleShortInfoAsync(
        string tagebuchnummer,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(tagebuchnummer))
            throw new ArianaLabException("tagebuchnummer darf nicht leer sein.");

        var requestUri = ArianaLabUriHelper.BuildSamplePath(tagebuchnummer, "Kurzinformation");
        try
        {
            return await GetAsync(requestUri, cancellationToken);
        }
        catch (ArianaLabException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new ArianaLabException(
                $"Keine Kurzinformation zur Probe '{tagebuchnummer}' gefunden.",
                HttpStatusCode.NotFound,
                ex);
        }
    }

    public async Task<string> GetReportJsonBySampleAsync(
        string tagebuchnummer,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(tagebuchnummer))
            throw new ArianaLabException("tagebuchnummer darf nicht leer sein.");

        var requestUri = ArianaLabUriHelper.BuildSamplePath(tagebuchnummer, "Pruefbericht/Exportable/Json");
        try
        {
            return await GetAsync(requestUri, cancellationToken);
        }
        catch (ArianaLabException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new ArianaLabException(
                $"Kein Prüfbericht für die Probe '{tagebuchnummer}' gefunden.",
                HttpStatusCode.NotFound,
                ex);
        }
    }

    public async Task<string> GetSampleResultsAsync(
        string tagebuchnummer,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(tagebuchnummer))
            throw new ArianaLabException("tagebuchnummer darf nicht leer sein.");

        var encoded = ArianaLabUriHelper.EncodePathSegment(tagebuchnummer);
        var requestUri = $"Rest/Opd/Probenbearbeitung/{encoded}";

        try
        {
            return await GetAsync(requestUri, cancellationToken);
        }
        catch (ArianaLabException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new ArianaLabException(
                $"Keine Bearbeitungs- und Ergebnisdaten für die Probe '{tagebuchnummer}' gefunden.",
                HttpStatusCode.NotFound,
                ex);
        }
    }

    public async Task<string> SearchSamplesAsync(
        string? tagebuchnummer,
        string? auftraggeber,
        string? rechnungsempfaenger,
        string? kundenprobennummer,
        string? probenbezeichnung,
        string? fertiggemeldetAmVon,
        string? fertiggemeldetAmBis,
        string? fertiggemeldetVon,
        string? kategorie,
        string? pruefkategorie,
        IReadOnlyList<SampleAdditionalFieldFilter>? zusatzfelder,
        bool? hauptprobe = true,
        bool? etikettiert = null,
        bool? erledigt = null,
        bool? beurteilt = null,
        bool? geprueft = null,
        bool? fertiggemeldet = null,
        bool? archiviert = null,
        bool? storniert = false,
        bool? freigabe = null,
        bool? eilig = null,
        bool? fakturiert = null,
        IReadOnlyList<string>? pruefpakete = null,
        string? kundengruppe1 = null,
        IReadOnlyList<string>? probengruppen = null,
        string? produktgruppe = null,
        string? probeneingangVon = null,
        string? probeneingangBis = null,
        string? terminVon = null,
        string? terminBis = null,
        string? erfasstAmVon = null,
        string? erfasstAmBis = null,
        string? erfasstVon = null,
        string? etikettiertAmVon = null,
        string? etikettiertAmBis = null,
        string? etikettiertVon = null,
        IReadOnlyList<string>? abteilungen = null,
        int limit = DefaultSearchLimit,
        CancellationToken cancellationToken = default)
    {
        limit = Math.Clamp(limit, 1, MaxSearchLimit);
        var conditions = new List<EasyQueryCondition>();

        AddContains("tagebuchnummer", tagebuchnummer);
        AddContains("auftraggeber", auftraggeber);
        AddContains("rechnungsempfaenger", rechnungsempfaenger);
        AddContains("kundenprobennummer", kundenprobennummer);
        AddContains("probenbezeichnung", probenbezeichnung);
        AddDateRange("fertiggemeldetam", fertiggemeldetAmVon, fertiggemeldetAmBis);
        AddContains("fertiggemeldetvon", fertiggemeldetVon);
        AddContains("kategorie", kategorie);
        AddContains("pruefkategorie", pruefkategorie);
        AddAdditionalFields(zusatzfelder);
        AddBoolean("ishauptprobe", hauptprobe);
        AddBoolean("etikettiert", etikettiert);
        AddBoolean("erledigt", erledigt);
        AddBoolean("beurteilt", beurteilt);
        AddBoolean("geprueft", geprueft);
        AddBoolean("fertiggemeldet", fertiggemeldet);
        AddBoolean("archiviert", archiviert);
        AddBoolean("storniert", storniert);
        AddBoolean("freigabe", freigabe);
        AddBoolean("eilig", eilig);
        AddBoolean("fakturiert", fakturiert);
        AddIdentifiers("pruefpaket", pruefpakete);
        AddContains("kundengruppe1", kundengruppe1);
        AddAny("probengruppe", probengruppen);
        AddContains("produktgruppe", produktgruppe);
        AddDateRange("probeneingang", probeneingangVon, probeneingangBis);
        AddDateRange("termin", terminVon, terminBis);
        AddDateRange("erfasstam", erfasstAmVon, erfasstAmBis);
        AddContains("erfasstvon", erfasstVon);
        AddDateRange("etikettiertam", etikettiertAmVon, etikettiertAmBis);
        AddContains("etikettiertvon", etikettiertVon);
        AddIdentifiers("abteilung", abteilungen);

        if (conditions.Count == 0)
            throw new ArianaLabException("Mindestens ein Suchkriterium muss angegeben werden.");

        var query = EasyQueryBuilder.BuildJson(
            conditions,
            limit,
            sortings: [new EasyQuerySorting("Tagebuchnummer", "desc")]);
        var body = await GetQueryAsync("Rest/Opd/Proben", query, cancellationToken);
        return HalResponseHelper.ProjectCompact(
            body,
            ["Tagebuchnummer", "Auftraggeber", "Probenbezeichnung", "Kundenprobennummer", "Status"],
            limit);

        void AddContains(string property, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                conditions.Add(EasyQueryCondition.Contains(property, value.Trim()));
        }

        void AddBoolean(string property, bool? value)
        {
            if (value.HasValue)
                conditions.Add(EasyQueryCondition.Equals(property, value.Value ? "1" : "0"));
        }

        void AddDateRange(string property, string? from, string? to)
        {
            if (!string.IsNullOrWhiteSpace(from))
                conditions.Add(EasyQueryCondition.GreaterOrEqual($"{property}von", from.Trim()));
            if (!string.IsNullOrWhiteSpace(to))
                conditions.Add(EasyQueryCondition.LessOrEqual($"{property}bis", to.Trim()));
        }

        void AddAdditionalFields(IReadOnlyList<SampleAdditionalFieldFilter>? fields)
        {
            foreach (var field in fields ?? [])
            {
                if (field is null)
                    throw new ArianaLabException("Zusatzfelder dürfen nicht null sein.");
                if (string.IsNullOrWhiteSpace(field.Bezeichnung) || string.IsNullOrWhiteSpace(field.Inhalt))
                    throw new ArianaLabException("Jedes Zusatzfeld benötigt Bezeichnung und Inhalt.");
                if (field.Bezeichnung.IndexOfAny(['[', ']']) >= 0)
                    throw new ArianaLabException("Zusatzfeld-Bezeichnungen dürfen keine eckigen Klammern enthalten.");

                var op = string.IsNullOrWhiteSpace(field.Operator) ? "~*" : field.Operator.Trim();
                if (!AdditionalFieldOperators.Contains(op))
                    throw new ArianaLabException(
                        $"Ungültiger Zusatzfeld-Operator '{op}'. Erlaubt: {string.Join(", ", AdditionalFieldOperators)}.");

                var pattern = field.Inhalt.Trim();
                if (op == "~*" && !pattern.Contains('*'))
                    pattern = $"*{pattern}*";

                var name = field.Bezeichnung.Trim().Replace("'", "''", StringComparison.Ordinal);
                conditions.Add(new EasyQueryCondition($"zusatzfeld[{name}]", op, pattern));
            }
        }

        void AddIdentifiers(string property, IReadOnlyList<string>? identifiers)
        {
            var values = identifiers?
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Select(value => value.Trim().Replace("'", "''", StringComparison.Ordinal))
                .ToArray();
            if (values is { Length: > 0 })
                conditions.Add(EasyQueryCondition.Equals(
                    $"{property}[{string.Join("','", values)}]",
                    "1"));
        }

        void AddAny(string property, IReadOnlyList<string>? values)
        {
            var patterns = values?
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Select(value => value.Trim())
                .ToArray();
            if (patterns is { Length: > 0 })
                conditions.Add(new EasyQueryCondition(property, "()", patterns));
        }
    }

    public async Task<string> GetSampleLogsAsync(
        string tagebuchnummer,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(tagebuchnummer))
            throw new ArianaLabException("tagebuchnummer darf nicht leer sein.");

        var requestUri = ArianaLabUriHelper.BuildSamplePath(tagebuchnummer, "Logs");
        try
        {
            return await GetAsync(requestUri, cancellationToken);
        }
        catch (ArianaLabException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new ArianaLabException(
                $"Kein Änderungsprotokoll für die Probe '{tagebuchnummer}' gefunden.",
                HttpStatusCode.NotFound,
                ex);
        }
    }

    public async Task<string> GetSampleAttachmentsAsync(
        string tagebuchnummer,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(tagebuchnummer))
            throw new ArianaLabException("tagebuchnummer darf nicht leer sein.");

        var encoded = ArianaLabUriHelper.EncodePathSegment(tagebuchnummer);
        var requestUri = $"Rest/Opd/Attachments/Proben/{encoded}";

        try
        {
            var body = await GetAsync(requestUri, cancellationToken);
            return HalResponseHelper.EnsureEmbeddedOrReturnRaw(body);
        }
        catch (ArianaLabException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new ArianaLabException(
                $"Keine Anhänge für die Probe '{tagebuchnummer}' gefunden.",
                HttpStatusCode.NotFound,
                ex);
        }
    }

}

public sealed class SampleAdditionalFieldFilter
{
    [JsonPropertyName("Bezeichnung")]
    [Description("Bezeichnung des Zusatzfelds, zum Beispiel 'Artikelnummer'.")]
    public string Bezeichnung { get; init; } = string.Empty;

    [JsonPropertyName("Operator")]
    [Description("Vergleichsoperator: >, >=, <, <=, =, ~* oder !=. Standard ist ~* (enthält).")]
    public string Operator { get; init; } = "~*";

    [JsonPropertyName("Inhalt")]
    [Description("Gesuchter Inhalt des Zusatzfelds.")]
    public string Inhalt { get; init; } = string.Empty;
}
