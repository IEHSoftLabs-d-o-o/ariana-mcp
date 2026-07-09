using Ariana_Mcp.integrations.Helpers;

namespace Ariana_Mcp.integrations.Services;

public sealed class SensitiveDataTools(SensitiveDataGuard guard)
{
    public void EnsureSensitiveDataEnabled(string featureName) => guard.EnsureEnabled(featureName);
}
