namespace Ariana_Mcp.Integrations.AraianLab;

public enum ArianaLabTokenStatus
{
    Missing,
    Invalid,
    Expired,
    Valid,
}

public sealed class ArianaLabIssuedToken
{
    public required string Token { get; init; }

    public required DateTimeOffset ExpireDate { get; init; }
}

public sealed class ArianaLabTokenValidation
{
    public ArianaLabTokenStatus Status { get; init; }

    public string? User { get; init; }

    public string? Password { get; init; }

    public DateTimeOffset? ExpireDate { get; init; }

    public bool IsValid => Status == ArianaLabTokenStatus.Valid;
}

public interface IArianaLabTokenService
{
    ArianaLabIssuedToken Issue(string user, string password);

    Task<ArianaLabTokenValidation> ValidateAuthorizationHeaderAsync(string? authorizationHeader);
}
