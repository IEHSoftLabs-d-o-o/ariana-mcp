using Ariana_Mcp.integrations.Exceptions;
using Ariana_Mcp.integrations.Helpers;
using ModelContextProtocol;

namespace Ariana_Mcp.Mcp;

internal static class McpToolRunner
{
    public static Task<string> RunAsync(Func<Task<string>> action) =>
        RunAsync(action, CancellationToken.None);

    public static async Task<string> RunAsync(Func<Task<string>> action, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            var result = await action().ConfigureAwait(false);
            return JsonResponseCleaner.Clean(result);
        }
        catch (ArianaLabException ex)
        {
            throw new McpException(ex.Message, ex);
        }
    }
}
