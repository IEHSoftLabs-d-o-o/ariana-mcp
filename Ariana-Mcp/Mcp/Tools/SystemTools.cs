using System.ComponentModel;
using Ariana_Mcp.integrations.Services;
using Ariana_Mcp.Mcp;
using ModelContextProtocol.Server;

namespace Ariana_Mcp.Mcp.Tools;

[McpServerToolType]
public sealed class SystemTools(SystemService systemService)
{
    [McpServerTool(Name = "get_system_info", Title = "Systemdiagnose", ReadOnly = true, Idempotent = true)]
    [Description(
        "Prüft, ob ArianaLab erreichbar ist und mit welchem Benutzer der MCP verbunden ist. " +
        "Verwenden zur Diagnose, wenn Tools nicht funktionieren oder Berechtigungen unklar sind.")]
    public Task<string> GetSystemInfo(CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => systemService.GetSystemInfoAsync(cancellationToken),
            cancellationToken);
}
