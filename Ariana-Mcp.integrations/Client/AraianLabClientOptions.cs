using System.ComponentModel.DataAnnotations;

namespace Ariana_Mcp.Integrations.AraianLab;

public sealed class AraianLabClientOptions
{
    public const string SectionName = "AraianLab";

    public string User { get; set; } = "";

    public string Password { get; set; } = "";

    /// <summary>Base URL for ArianaLab requests (e.g. https://klims.labor-kneissler.de/).</summary>
    [Required]
    [Url]
    public string BaseUrl { get; set; } = "";
}
