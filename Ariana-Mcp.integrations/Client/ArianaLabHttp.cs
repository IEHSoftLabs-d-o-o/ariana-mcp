namespace Ariana_Mcp.Integrations.AraianLab;

/// <summary>Name registered with <see cref="IHttpClientFactory"/> for the lab API (Bearer auth from configuration).</summary>
public static class ArianaLabHttp
{
    public const string ClientName = "ArianaLabClient";

    public const string LoginClientName = "ArianaLabLoginClient";

    public const string HomeLoginPath = "Home/Login";
}
