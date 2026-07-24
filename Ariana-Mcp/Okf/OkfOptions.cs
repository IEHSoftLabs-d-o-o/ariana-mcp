using System.ComponentModel.DataAnnotations;

namespace Ariana_Mcp.Okf;

public sealed class OkfOptions
{
    public const string SectionName = "Okf";

    /// <summary>
    /// Path to the OKF bundle root. Relative paths are resolved against the application base directory.
    /// </summary>
    [Required]
    public string BundlePath { get; set; } = "Okf/klims";
}
