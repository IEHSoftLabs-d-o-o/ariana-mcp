using System.ComponentModel.DataAnnotations;

namespace Ariana_Mcp.Integrations.AraianLab;

public sealed class AraianLabClientOptions
{
    public const string SectionName = "AraianLab";

    /// <summary>Base URL for ArianaLab requests (e.g. https://klims.labor-kneissler.de/).</summary>
    [Required]
    [Url]
    public string BaseUrl { get; set; } = "";

    public ArianaLabJwtOptions Jwt { get; set; } = new();
}

public sealed class ArianaLabJwtOptions
{
    public string SigningKey { get; set; } = "";

    public string EncryptionKey { get; set; } = "";

    public int LifetimeHours { get; set; } = 17520;

    public string Issuer { get; set; } = "ariana-mcp";

    public string Audience { get; set; } = "ariana-mcp";
}
