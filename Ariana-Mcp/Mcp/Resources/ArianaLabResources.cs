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
        Name = "Sample",
        MimeType = "application/json")]
    [Description(
        "Loads the full data for a lab sample by lab journal number. " +
        "Use when details for a specific sample are needed, for example '26-0318054'.")]
    public Task<TextResourceContents> GetSample(string tagebuchnummer, CancellationToken cancellationToken)
        => McpResourceRunner.RunAsync(async () =>
        {
            var decoded = ArianaLabUriHelper.DecodePathSegment(tagebuchnummer);
            var body = await sampleService.GetSampleByIdAsync(decoded, cancellationToken);
            return CreateJsonResource($"arianalab://sample/{tagebuchnummer}", body);
        }, cancellationToken);

    [McpServerResource(
        UriTemplate = "arianalab://sample/{tagebuchnummer}/logs",
        Name = "Sample change log",
        MimeType = "application/json")]
    [Description(
        "Loads the change log for a sample. Use only when explicitly asked who changed something on the sample and when; " +
        "may contain internal audit data.")]
    public Task<TextResourceContents> GetSampleLogs(string tagebuchnummer, CancellationToken cancellationToken)
        => McpResourceRunner.RunAsync(async () =>
        {
            sensitiveDataGuard.EnsureEnabled("sample logs");
            var decoded = ArianaLabUriHelper.DecodePathSegment(tagebuchnummer);
            var body = await sampleService.GetSampleLogsAsync(decoded, cancellationToken);
            return CreateJsonResource($"arianalab://sample/{tagebuchnummer}/logs", body);
        }, cancellationToken);

    [McpServerResource(
        UriTemplate = "arianalab://sample/{tagebuchnummer}/attachments",
        Name = "Sample attachments",
        MimeType = "application/json")]
    [Description(
        "Lists attachments for a sample, for example documents or images. Metadata only by default; " +
        "load content only when the user explicitly asks for the attachment.")]
    public Task<TextResourceContents> GetSampleAttachments(string tagebuchnummer, CancellationToken cancellationToken)
        => McpResourceRunner.RunAsync(async () =>
        {
            sensitiveDataGuard.EnsureEnabled("sample attachments");
            var decoded = ArianaLabUriHelper.DecodePathSegment(tagebuchnummer);
            var body = await sampleService.GetSampleAttachmentsAsync(decoded, cancellationToken);
            return CreateJsonResource($"arianalab://sample/{tagebuchnummer}/attachments", body);
        }, cancellationToken);

    [McpServerResource(
        UriTemplate = "arianalab://customer/{nummer}",
        Name = "Customer",
        MimeType = "application/json")]
    [Description(
        "Loads the customer master record for a customer number. May contain addresses, contacts, and other personal data.")]
    public Task<TextResourceContents> GetCustomer(string nummer, CancellationToken cancellationToken)
        => McpResourceRunner.RunAsync(async () =>
        {
            var body = await customerService.GetCustomerAsync(nummer, cancellationToken);
            return CreateJsonResource($"arianalab://customer/{nummer}", body);
        }, cancellationToken);

    [McpServerResource(
        UriTemplate = "arianalab://analysis/{id}",
        Name = "Analysis",
        MimeType = "application/json")]
    [Description(
        "Loads information about an analysis or test, for example name, description, and possible method assignments.")]
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
        "Loads a customer order / Customer Order Request. Use only when the user explicitly asks for a COR order; " +
        "may contain invoice and payment data.")]
    public Task<TextResourceContents> GetCor(string corId, CancellationToken cancellationToken)
        => McpResourceRunner.RunAsync(async () =>
        {
            sensitiveDataGuard.EnsureEnabled("COR details");
            var decoded = ArianaLabUriHelper.DecodePathSegment(corId);
            var body = await corService.GetCorAsync(decoded, cancellationToken);
            return CreateJsonResource($"arianalab://cor/{corId}", body);
        }, cancellationToken);

    [McpServerResource(
        UriTemplate = "arianalab://invoice/{id}",
        Name = "Invoice",
        MimeType = "application/json")]
    [Description(
        "Loads an invoice by invoice number. Use only when the user explicitly needs invoice data; " +
        "contains billing-relevant data.")]
    public Task<TextResourceContents> GetInvoice(string id, CancellationToken cancellationToken)
        => McpResourceRunner.RunAsync(async () =>
        {
            sensitiveDataGuard.EnsureEnabled("invoice details");
            var body = await invoiceService.GetInvoiceAsync(id, cancellationToken);
            return CreateJsonResource($"arianalab://invoice/{id}", body);
        }, cancellationToken);

    [McpServerResource(
        UriTemplate = "arianalab://planning/{module}/{id}",
        Name = "Planning order",
        MimeType = "application/json")]
    [Description(
        "Loads a planning or order record from a specified module, for example sample intake or customer orders.")]
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
        Text = JsonResponseCleaner.Clean(body),
    };
}
