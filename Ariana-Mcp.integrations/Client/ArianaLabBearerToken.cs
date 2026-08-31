using System.Text;

namespace Ariana_Mcp.Integrations.AraianLab;

public sealed class ArianaLabAccessToken
{
    public required string User { get; init; }

    public required string Password { get; init; }

    public required string Value { get; init; }

    public required string Credentials { get; init; }

    public required DateTimeOffset ExpireDate { get; init; }

    public bool IsExpired => DateTimeOffset.UtcNow >= ExpireDate;
}

public static class ArianaLabBearerToken
{
    public static ArianaLabAccessToken Create(string user, string password, TimeSpan lifetime)
    {
        var expireDate = DateTimeOffset.UtcNow.Add(lifetime);
        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user}:{password}"));
        return new ArianaLabAccessToken
        {
            User = user,
            Password = password,
            Credentials = credentials,
            ExpireDate = expireDate,
            Value = $"{credentials}.{expireDate.ToUnixTimeSeconds()}",
        };
    }

    public static bool TryParse(string? authorizationHeader, out ArianaLabAccessToken? token)
    {
        token = null;
        if (string.IsNullOrWhiteSpace(authorizationHeader))
            return false;

        var value = authorizationHeader.Trim();
        const string prefix = "Bearer ";
        while (value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            value = value[prefix.Length..].Trim();

        var separator = value.LastIndexOf('.');
        if (separator <= 0 || separator >= value.Length - 1)
            return false;

        var credentials = value[..separator];
        if (!long.TryParse(value[(separator + 1)..], out var expireUnix))
            return false;

        try
        {
            var raw = Encoding.UTF8.GetString(Convert.FromBase64String(credentials));
            var userSeparator = raw.IndexOf(':');
            if (userSeparator <= 0 || userSeparator >= raw.Length - 1)
                return false;

            var user = raw[..userSeparator];
            var password = raw[(userSeparator + 1)..];
            if (user.Length == 0 || password.Length == 0)
                return false;

            token = new ArianaLabAccessToken
            {
                User = user,
                Password = password,
                Credentials = credentials,
                ExpireDate = DateTimeOffset.FromUnixTimeSeconds(expireUnix),
                Value = value,
            };
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
