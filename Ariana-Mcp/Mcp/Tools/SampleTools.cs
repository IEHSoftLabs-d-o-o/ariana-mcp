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
        Title = "Probe nach ID",
        ReadOnly = true,
        Idempotent = true,
        Destructive = false)]
    [Description("Sucht eine Laborprobe (Probe) anhand der ID und gibt das JSON aus ArianaLab zurück.")]
    public Task<string> SampleById(
        [Description("Proben-ID im Format 'JJ-NNNNNNN', z. B. '26-0318054'.")]
        string sampleId,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => sampleService.GetSampleByIdAsync(sampleId, cancellationToken),
            cancellationToken);
}
