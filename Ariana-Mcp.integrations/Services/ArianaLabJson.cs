using System.Text.Json;

namespace Ariana_Mcp.integrations.Services;

internal static class ArianaLabJson
{
    public static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNamingPolicy = null,
        WriteIndented = false,
    };

    public static string Serialize(object value) =>
        JsonSerializer.Serialize(value, SerializerOptions);
}
