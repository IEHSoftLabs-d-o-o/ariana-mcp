using System.ComponentModel;
using Ariana_Mcp.Mcp;
using Ariana_Mcp.Okf;
using ModelContextProtocol.Server;

namespace Ariana_Mcp.Mcp.Tools;

[McpServerToolType]
public sealed class OkfTools(OkfService okfService)
{
    [McpServerTool(
        Name = "okf_bundle_status",
        Title = "OKF bundle status",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description("Inspect the configured OKF bundle and its root index.")]
    public Task<string> BundleStatus(CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => okfService.GetBundleStatusAsync(cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "okf_read_index",
        Title = "Read OKF index",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description("Read the root or nearest area index. Use this before concepts.")]
    public Task<string> ReadIndex(
        [Description("Optional area name or opaque index reference from a previous index. Omit for the root index.")]
        string? reference = null,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => okfService.ReadIndexAsync(reference, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "okf_read_concept",
        Title = "Read OKF concept",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description("Read one concept by its opaque reference. References are internal and must never be shown to the user.")]
    public Task<string> ReadConcept(
        [Description("Opaque concept reference from okf_search or an index, for example 'okf-1a2b3c4d5e'.")]
        string reference,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => okfService.ReadConceptAsync(reference, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "okf_search",
        Title = "Search OKF bundle",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description("Search OKF metadata and content. Returns opaque references for okf_read_concept.")]
    public Task<string> Search(
        [Description("Search query across metadata and documentation content.")]
        string query,
        [Description("Maximum number of matches (1-20, default 8).")]
        int limit = 8,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => okfService.SearchAsync(query, limit, cancellationToken),
            cancellationToken);
}
