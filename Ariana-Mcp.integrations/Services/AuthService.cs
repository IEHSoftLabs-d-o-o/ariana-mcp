using Ariana_Mcp.integrations.Models;
using Ariana_Mcp.Integrations.AraianLab;

namespace Ariana_Mcp.integrations.Services;

public sealed class AuthService(IHttpClientFactory httpClientFactory, IArianaLabTokenService tokenService)
{
    public async Task<LoginResponse> LoginAsync(
        string? user,
        string? password,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(password))
        {
            return new LoginResponse
            {
                ErrorMessage = "User and Password are required.",
            };
        }

        var client = httpClientFactory.CreateClient(ArianaLabHttp.LoginClientName);
        using var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["Name"] = user,
            ["Password"] = password,
        });

        using var response = await client
            .PostAsync(ArianaLabHttp.HomeLoginPath, content, cancellationToken)
            .ConfigureAwait(false);

        if (!IsRedirect(response.StatusCode))
        {
            return new LoginResponse
            {
                ErrorMessage = "Login failed. Check user name and password.",
            };
        }

        var issued = tokenService.Issue(user, password);
        return new LoginResponse
        {
            ErrorMessage = string.Empty,
            LoginToken = new LoginTokenResponse
            {
                Token = issued.Token,
                ExpireDate = issued.ExpireDate,
            },
        };
    }

    private static bool IsRedirect(System.Net.HttpStatusCode statusCode)
    {
        var status = (int)statusCode;
        return status is >= 300 and < 400;
    }
}
