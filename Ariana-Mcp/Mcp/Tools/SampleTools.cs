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
    [Description(
        "Sucht eine oder mehrere Laborproben (Proben) anhand ihrer IDs und gibt pro ID ein Ergebnis mit ArianaLab-JSON zurück. " +
        "Fehlende oder ungültige IDs führen nicht zum Abbruch der gesamten Anfrage.")]
    public Task<string> SampleById(
        [Description("Liste von Proben-IDs im Format 'JJ-NNNNNNN', z. B. ['26-0318054', '26-0318055'].")]
        IReadOnlyList<string> sampleIds,
        CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => sampleService.GetSamplesByIdsAsync(sampleIds, cancellationToken),
            cancellationToken);
}
