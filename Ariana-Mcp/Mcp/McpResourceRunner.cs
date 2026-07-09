using Ariana_Mcp.integrations.Exceptions;
using ModelContextProtocol;

namespace Ariana_Mcp.Mcp;

internal static class McpResourceRunner
{
    public static async Task<T> RunAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            return await action().ConfigureAwait(false);
        }
        catch (ArianaLabException ex)
        {
            throw new McpException(ex.Message, ex);
        }
    }
}
