using System.ComponentModel;
using Ariana_Mcp.integrations.Services;
using Ariana_Mcp.Mcp;
using ModelContextProtocol.Server;

namespace Ariana_Mcp.Mcp.Tools;

[McpServerToolType]
public sealed class SampleTools(SampleService sampleService)
{
    [McpServerTool(
        Name = "sample_by_id",
        Title = "Sample by id",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description("Looks up a laboratory sample (Probe) by id and returns the JSON from ArianaLab.")]
    public Task<string> SampleById(
        [Description("Sample (Probe) id in the format 'YY-NNNNNNN', e.g. '26-0318054'.")]
        string sampleId,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => sampleService.GetSampleByIdAsync(sampleId, cancellationToken),
            cancellationToken);
}
