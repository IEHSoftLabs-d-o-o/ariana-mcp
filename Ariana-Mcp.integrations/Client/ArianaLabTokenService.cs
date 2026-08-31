using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Ariana_Mcp.Integrations.AraianLab;

public sealed class ArianaLabTokenService : IArianaLabTokenService
{
    public const string LabUserClaim = "lab_user";
    public const string ProtectedPasswordClaim = "cred";

    private readonly ArianaLabJwtOptions _jwt;
    private readonly SymmetricSecurityKey _signingKey;
    private readonly byte[] _encryptionKey;
    private readonly JsonWebTokenHandler _handler = new();

    public ArianaLabTokenService(IOptions<AraianLabClientOptions> options)
    {
        _jwt = options.Value.Jwt;
        if (_jwt.LifetimeHours <= 0)
            _jwt.LifetimeHours = 17520;

        _signingKey = new SymmetricSecurityKey(ResolveKeyBytes(_jwt.SigningKey, 32));
        _encryptionKey = ResolveKeyBytes(_jwt.EncryptionKey, 32);
    }

    public ArianaLabIssuedToken Issue(string user, string password)
    {
        var expireDate = DateTimeOffset.UtcNow.AddHours(_jwt.LifetimeHours);
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, user),
                new Claim(LabUserClaim, user),
                new Claim(ProtectedPasswordClaim, PasswordProtector.Encrypt(password, _encryptionKey)),
            ]),
            Expires = expireDate.UtcDateTime,
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow,
            Issuer = _jwt.Issuer,
            Audience = _jwt.Audience,
            SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256),
        };

        return new ArianaLabIssuedToken
        {
            Token = _handler.CreateToken(descriptor),
            ExpireDate = expireDate,
        };
    }

    public async Task<ArianaLabTokenValidation> ValidateAuthorizationHeaderAsync(string? authorizationHeader)
    {
        var compact = UnwrapBearer(authorizationHeader);
        if (string.IsNullOrEmpty(compact))
        {
            return new ArianaLabTokenValidation { Status = ArianaLabTokenStatus.Missing };
        }

        var result = await _handler.ValidateTokenAsync(compact, new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _jwt.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwt.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _signingKey,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
        });

        if (!result.IsValid)
        {
            return new ArianaLabTokenValidation
            {
                Status = IsExpired(result) ? ArianaLabTokenStatus.Expired : ArianaLabTokenStatus.Invalid,
            };
        }

        var user = result.ClaimsIdentity?.FindFirst(LabUserClaim)?.Value
            ?? result.ClaimsIdentity?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        var protectedPassword = result.ClaimsIdentity?.FindFirst(ProtectedPasswordClaim)?.Value;
        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(protectedPassword))
        {
            return new ArianaLabTokenValidation { Status = ArianaLabTokenStatus.Invalid };
        }

        try
        {
            return new ArianaLabTokenValidation
            {
                Status = ArianaLabTokenStatus.Valid,
                User = user,
                Password = PasswordProtector.Decrypt(protectedPassword, _encryptionKey),
                ExpireDate = result.SecurityToken is JsonWebToken jwt
                    ? new DateTimeOffset(DateTime.SpecifyKind(jwt.ValidTo, DateTimeKind.Utc))
                    : DateTimeOffset.UtcNow.AddHours(_jwt.LifetimeHours),
            };
        }
        catch (Exception)
        {
            return new ArianaLabTokenValidation { Status = ArianaLabTokenStatus.Invalid };
        }
    }

    private static bool IsExpired(TokenValidationResult result)
    {
        for (var exception = result.Exception; exception is not null; exception = exception.InnerException)
        {
            if (exception is SecurityTokenExpiredException)
                return true;
        }

        return false;
    }

    private static string? UnwrapBearer(string? authorizationHeader)
    {
        if (string.IsNullOrWhiteSpace(authorizationHeader))
            return null;

        var value = authorizationHeader.Trim();
        const string prefix = "Bearer ";
        while (value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            value = value[prefix.Length..].Trim();

        return string.IsNullOrEmpty(value) ? null : value;
    }

    private static byte[] ResolveKeyBytes(string configured, int size)
    {
        if (string.IsNullOrWhiteSpace(configured))
            return RandomNumberGenerator.GetBytes(size);

        var raw = Encoding.UTF8.GetBytes(configured);
        if (raw.Length >= size)
            return raw[..size];

        return SHA256.HashData(raw);
    }
}
