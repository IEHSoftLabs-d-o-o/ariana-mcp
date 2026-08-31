using System.Text;

namespace Ariana_Mcp.Integrations.AraianLab;

public static class ArianaLabBearerToken
{
    public static string Create(string user, string password) =>
        Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user}:{password}"));

    public static bool TryRead(
        string? authorizationHeader,
        out string user,
        out string password,
        out string token)
    {
        user = "";
        password = "";
        token = "";

        if (string.IsNullOrWhiteSpace(authorizationHeader))
            return false;

        var value = authorizationHeader.Trim();
        const string prefix = "Bearer ";
        while (value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            value = value[prefix.Length..].Trim();

        if (string.IsNullOrEmpty(value))
            return false;

        token = value;

        try
        {
            var raw = Encoding.UTF8.GetString(Convert.FromBase64String(value));
            var separator = raw.IndexOf(':');
            if (separator <= 0 || separator >= raw.Length - 1)
                return false;

            user = raw[..separator];
            password = raw[(separator + 1)..];
            return user.Length > 0 && password.Length > 0;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
