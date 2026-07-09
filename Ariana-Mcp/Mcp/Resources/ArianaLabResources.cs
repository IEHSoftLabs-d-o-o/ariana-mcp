using System.ComponentModel;
using Ariana_Mcp.integrations.Helpers;
using Ariana_Mcp.integrations.Services;
using Ariana_Mcp.Mcp;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace Ariana_Mcp.Mcp.Resources;

[McpServerResourceType]
public sealed class ArianaLabResources(
    SampleService sampleService,
    CustomerService customerService,
    ReferenceDataService referenceDataService,
    OrderService orderService,
    CorService corService,
    InvoiceService invoiceService,
    SensitiveDataGuard sensitiveDataGuard)
{
    [McpServerResource(
        UriTemplate = "arianalab://sample/{tagebuchnummer}",
        Name = "Probe",
        MimeType = "application/json")]
    [Description(
        "Lädt die vollständigen Daten zu einer Laborprobe anhand der Tagebuchnummer. " +
        "Verwenden, wenn Details zu einer bestimmten Probe benötigt werden, z. B. '26-0318054'.")]
    public Task<TextResourceContents> GetSample(string tagebuchnummer, CancellationToken cancellationToken)
        => McpResourceRunner.RunAsync(async () =>
        {
            var decoded = ArianaLabUriHelper.DecodePathSegment(tagebuchnummer);
            var body = await sampleService.GetSampleByIdAsync(decoded, cancellationToken);
            return CreateJsonResource($"arianalab://sample/{tagebuchnummer}", body);
        }, cancellationToken);

    [McpServerResource(
        UriTemplate = "arianalab://sample/{tagebuchnummer}/logs",
        Name = "Probe Änderungsprotokoll",
        MimeType = "application/json")]
    [Description(
        "Lädt das Änderungsprotokoll zu einer Probe. Nur verwenden, wenn ausdrücklich gefragt wird, wer wann etwas an der Probe geändert hat; " +
        "kann interne Auditdaten enthalten.")]
    public Task<TextResourceContents> GetSampleLogs(string tagebuchnummer, CancellationToken cancellationToken)
        => McpResourceRunner.RunAsync(async () =>
        {
            sensitiveDataGuard.EnsureEnabled("Probe-Logs");
            var decoded = ArianaLabUriHelper.DecodePathSegment(tagebuchnummer);
            var body = await sampleService.GetSampleLogsAsync(decoded, cancellationToken);
            return CreateJsonResource($"arianalab://sample/{tagebuchnummer}/logs", body);
        }, cancellationToken);

    [McpServerResource(
        UriTemplate = "arianalab://sample/{tagebuchnummer}/attachments",
        Name = "Probe Anhänge",
        MimeType = "application/json")]
    [Description(
        "Listet Anhänge zu einer Probe, z. B. Dokumente oder Bilder. Standardmäßig nur Metadaten; " +
        "Inhalte nur laden, wenn der Nutzer ausdrücklich nach dem Anhang fragt.")]
    public Task<TextResourceContents> GetSampleAttachments(string tagebuchnummer, CancellationToken cancellationToken)
        => McpResourceRunner.RunAsync(async () =>
        {
            sensitiveDataGuard.EnsureEnabled("Probe-Anhänge");
            var decoded = ArianaLabUriHelper.DecodePathSegment(tagebuchnummer);
            var body = await sampleService.GetSampleAttachmentsAsync(decoded, cancellationToken);
            return CreateJsonResource($"arianalab://sample/{tagebuchnummer}/attachments", body);
        }, cancellationToken);

    [McpServerResource(
        UriTemplate = "arianalab://customer/{nummer}",
        Name = "Kunde",
        MimeType = "application/json")]
    [Description(
        "Lädt den Kundenstammdatensatz zu einer Kundennummer. Kann Adressen, Ansprechpartner und weitere personenbezogene Daten enthalten.")]
    public Task<TextResourceContents> GetCustomer(string nummer, CancellationToken cancellationToken)
        => McpResourceRunner.RunAsync(async () =>
        {
            var body = await customerService.GetCustomerAsync(nummer, cancellationToken);
            return CreateJsonResource($"arianalab://customer/{nummer}", body);
        }, cancellationToken);

    [McpServerResource(
        UriTemplate = "arianalab://analysis/{id}",
        Name = "Analyse",
        MimeType = "application/json")]
    [Description(
        "Lädt Informationen zu einer Analyse oder Untersuchung, z. B. Name, Beschreibung und mögliche Zuordnung zu Methoden.")]
    public Task<TextResourceContents> GetAnalysis(string id, CancellationToken cancellationToken)
        => McpResourceRunner.RunAsync(async () =>
        {
            var body = await referenceDataService.GetAnalysisAsync(id, cancellationToken);
            return CreateJsonResource($"arianalab://analysis/{id}", body);
        }, cancellationToken);

    [McpServerResource(
        UriTemplate = "arianalab://cor/{corId}",
        Name = "Customer Order Request",
        MimeType = "application/json")]
    [Description(
        "Lädt einen Kundenauftrag / Customer Order Request. Nur verwenden, wenn der Nutzer ausdrücklich nach einem COR-Auftrag fragt; " +
        "kann Rechnungs- und Zahlungsdaten enthalten.")]
    public Task<TextResourceContents> GetCor(string corId, CancellationToken cancellationToken)
        => McpResourceRunner.RunAsync(async () =>
        {
            sensitiveDataGuard.EnsureEnabled("COR-Details");
            var decoded = ArianaLabUriHelper.DecodePathSegment(corId);
            var body = await corService.GetCorAsync(decoded, cancellationToken);
            return CreateJsonResource($"arianalab://cor/{corId}", body);
        }, cancellationToken);

    [McpServerResource(
        UriTemplate = "arianalab://invoice/{id}",
        Name = "Rechnung",
        MimeType = "application/json")]
    [Description(
        "Lädt eine Rechnung anhand der Rechnungsnummer. Nur verwenden, wenn der Nutzer ausdrücklich Rechnungsdaten benötigt; " +
        "enthält abrechnungsrelevante Daten.")]
    public Task<TextResourceContents> GetInvoice(string id, CancellationToken cancellationToken)
        => McpResourceRunner.RunAsync(async () =>
        {
            sensitiveDataGuard.EnsureEnabled("Rechnungsdetails");
            var body = await invoiceService.GetInvoiceAsync(id, cancellationToken);
            return CreateJsonResource($"arianalab://invoice/{id}", body);
        }, cancellationToken);

    [McpServerResource(
        UriTemplate = "arianalab://planning/{module}/{id}",
        Name = "Planungsauftrag",
        MimeType = "application/json")]
    [Description(
        "Lädt einen Planungs- oder Auftragsdatensatz aus einem angegebenen Modul, z. B. Probenanlage oder Kundenauftrag.")]
    public Task<TextResourceContents> GetPlanningOrder(
        string module,
        string id,
        CancellationToken cancellationToken)
        => McpResourceRunner.RunAsync(async () =>
        {
            var body = await orderService.GetPlanningOrderAsync(module, id, cancellationToken);
            return CreateJsonResource($"arianalab://planning/{module}/{id}", body);
        }, cancellationToken);

    private static TextResourceContents CreateJsonResource(string uri, string body) => new()
    {
        Uri = uri,
        MimeType = "application/json",
        Text = body,
    };
}
