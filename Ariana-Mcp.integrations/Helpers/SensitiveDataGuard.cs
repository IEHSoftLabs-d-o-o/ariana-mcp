using Ariana_Mcp.Integrations.AraianLab;
using Ariana_Mcp.integrations.Exceptions;
using Microsoft.Extensions.Options;

namespace Ariana_Mcp.integrations.Helpers;

public sealed class SensitiveDataGuard(IOptions<AraianLabClientOptions> options)
{
    public void EnsureEnabled(string featureName)
    {
        if (options.Value.EnableSensitiveData)
            return;

        throw new ArianaLabException(
            $"Der Zugriff auf '{featureName}' ist deaktiviert. " +
            "Setzen Sie AraianLab:EnableSensitiveData auf true, wenn diese Daten ausdrücklich benötigt werden.");
    }
}
