using System.Net;
using Ariana_Mcp.integrations.Exceptions;
using Ariana_Mcp.integrations.Helpers;

namespace Ariana_Mcp.integrations.Services;

public sealed class SampleService(IHttpClientFactory httpClientFactory)
    : ArianaLabServiceBase(httpClientFactory)
{
    private const int DefaultSearchLimit = 25;
    private const int MaxSearchLimit = 100;

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
        string? q,
        string? tagebuchnummer,
        string? kunde,
        string? kundenprobennummer,
        string? probenbezeichnung,
        string? von,
        string? bis,
        string? status,
        int limit = DefaultSearchLimit,
        CancellationToken cancellationToken = default)
    {
        limit = Math.Clamp(limit, 1, MaxSearchLimit);

        var query = q;
        if (string.IsNullOrWhiteSpace(query))
        {
            var filters = new Dictionary<string, string?>
            {
                ["tagebuchnummer"] = tagebuchnummer,
                ["auftraggeber"] = kunde,
                ["kundenprobennummer"] = kundenprobennummer,
                ["probenbezeichnung"] = probenbezeichnung,
            };

            var conditions = new List<EasyQueryCondition>();
            foreach (var (property, value) in filters)
            {
                if (string.IsNullOrWhiteSpace(value))
                    continue;

                conditions.Add(property == "tagebuchnummer"
                    ? EasyQueryCondition.Contains(property, value.Trim())
                    : EasyQueryCondition.Contains(property, value.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(von))
                conditions.Add(EasyQueryCondition.GreaterOrEqual("probeneingangvon", von.Trim()));

            if (!string.IsNullOrWhiteSpace(bis))
                conditions.Add(EasyQueryCondition.LessOrEqual("probeneingangbis", bis.Trim()));

            if (!string.IsNullOrWhiteSpace(status))
                MapStatusCondition(status.Trim(), conditions);

            if (conditions.Count == 0)
                throw new ArianaLabException(
                    "Mindestens ein Suchkriterium muss angegeben werden (q, tagebuchnummer, kunde, kundenprobennummer, probenbezeichnung, von, bis oder status).");

            query = EasyQueryBuilder.BuildJson(conditions, limit);
        }
        else
        {
            query = EasyQueryBuilder.EnsureLimit(query, limit);
        }

        var body = await GetQueryAsync("Rest/Opd/Proben", query, cancellationToken);
        return HalResponseHelper.ProjectCompact(
            body,
            ["Tagebuchnummer", "Auftraggeber", "Probenbezeichnung", "Kundenprobennummer", "Status"],
            limit);
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

    private static void MapStatusCondition(string status, List<EasyQueryCondition> conditions)
    {
        switch (status.ToLowerInvariant())
        {
            case "fertiggemeldet":
                conditions.Add(EasyQueryCondition.Equals("fertiggemeldet", "1"));
                break;
            case "beurteilt":
                conditions.Add(EasyQueryCondition.Equals("beurteilt", "1"));
                break;
            case "storniert":
                conditions.Add(EasyQueryCondition.Equals("storniert", "1"));
                break;
            case "archiviert":
                conditions.Add(EasyQueryCondition.Equals("archiviert", "1"));
                break;
            default:
                conditions.Add(EasyQueryCondition.Contains("probenbezeichnung", status));
                break;
        }
    }
}
