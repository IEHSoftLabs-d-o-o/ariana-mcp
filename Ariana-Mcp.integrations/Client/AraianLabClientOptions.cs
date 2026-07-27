using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ariana_Mcp.Integrations.AraianLab;

namespace Ariana_Mcp.Integrations.AraianLab;

public sealed class AraianLabClientOptions
{
    public const string SectionName = "AraianLab";

    [Required]
    public string User { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";

    /// <summary>Base URL for ArianaLab requests (e.g. https://klims.labor-kneissler.de/).</summary>
    [Required]
    [Url]
    public string BaseUrl { get; set; } = "";
}
