namespace Ariana_Mcp.integrations.Models;

public sealed class LoginRequest
{
    public string? User { get; set; }

    public string? Password { get; set; }
}

public sealed class LoginResponse
{
    public string? ErrorMessage { get; set; }

    public LoginTokenResponse? LoginToken { get; set; }
}

public sealed class LoginTokenResponse
{
    public string Token { get; set; } = "";

    public DateTimeOffset ExpireDate { get; set; }
}
