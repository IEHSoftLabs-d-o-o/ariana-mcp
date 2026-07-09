namespace Ariana_Mcp.integrations.Helpers;

public static class ArianaLabUriHelper
{
    public static string EncodePathSegment(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        return value.Replace("/", "~", StringComparison.Ordinal);
    }

    public static string DecodePathSegment(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        return value.Replace("~", "/", StringComparison.Ordinal);
    }

    public static string BuildSamplePath(string tagebuchnummer, string? suffix = null)
    {
        var encoded = EncodePathSegment(tagebuchnummer);
        return string.IsNullOrEmpty(suffix)
            ? $"Rest/Opd/Proben/{encoded}/"
            : $"Rest/Opd/Proben/{encoded}/{suffix.TrimStart('/')}";
    }
}
