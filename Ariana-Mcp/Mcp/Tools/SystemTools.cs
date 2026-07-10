using System.ComponentModel;
using Ariana_Mcp.integrations.Services;
using Ariana_Mcp.Mcp;
using ModelContextProtocol.Server;

namespace Ariana_Mcp.Mcp.Tools;

[McpServerToolType]
public sealed class SystemTools(SystemService systemService)
{
    [McpServerTool(Name = "get_system_info", Title = "System diagnostics", ReadOnly = true, Idempotent = true)]
    [Description(
        "Checks whether ArianaLab is reachable and which user the MCP is connected as. " +
        "Use for diagnosis when tools do not work or permissions are unclear.")]
    public Task<string> GetSystemInfo(CancellationToken cancellationToken = default)
        => McpToolRunner.RunAsync(
            () => systemService.GetSystemInfoAsync(cancellationToken),
            cancellationToken);
}
