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
    [Description("Read the root or nearest directory index. Use this before concepts.")]
    public Task<string> ReadIndex(
        [Description("Bundle-relative directory or index path.")]
        string? path = null,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => okfService.ReadIndexAsync(path, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "okf_read_concept",
        Title = "Read OKF concept",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description("Read one Markdown concept by bundle-relative path.")]
    public Task<string> ReadConcept(
        [Description("Bundle-relative Markdown path, for example 'workflows/proben-anlegen.md'.")]
        string path,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => okfService.ReadConceptAsync(path, cancellationToken),
            cancellationToken);

    [McpServerTool(
        Name = "okf_search",
        Title = "Search OKF bundle",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description("Search OKF paths, metadata, and content. Read relevant concepts afterward.")]
    public Task<string> Search(
        [Description("Search query across paths, metadata, and Markdown content.")]
        string query,
        [Description("Maximum number of matches (1-20, default 8).")]
        int limit = 8,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => okfService.SearchAsync(query, limit, cancellationToken),
            cancellationToken);
}
