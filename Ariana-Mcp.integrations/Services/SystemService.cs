using System.Text.Json;
using Ariana_Mcp.integrations.Exceptions;

namespace Ariana_Mcp.integrations.Services;

public sealed class SystemService(IHttpClientFactory httpClientFactory)
    : ArianaLabServiceBase(httpClientFactory)
{
    public async Task<string> GetSystemInfoAsync(CancellationToken cancellationToken = default)
    {
        var currentUser = await SafeGetAsync("Rest/Sys/Info/currentUser", cancellationToken);
        var user = await SafeGetAsync("Rest/User", cancellationToken);
        var keepAlive = await SafeGetAsync("Rest/Sys/KeepAlive", cancellationToken);

        return JsonSerializer.Serialize(new
        {
            currentUser,
            user,
            keepAlive,
        });
    }

    private async Task<object> SafeGetAsync(string uri, CancellationToken cancellationToken)
    {
        try
        {
            var body = await GetAsync(uri, cancellationToken);
            return JsonSerializer.Deserialize<JsonElement>(body);
        }
        catch (ArianaLabException ex)
        {
            return new { error = ex.Message, statusCode = ex.StatusCode?.ToString() };
        }
    }
}
