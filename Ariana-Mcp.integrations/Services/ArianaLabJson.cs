using System.Text.Json;
using Ariana_Mcp.integrations.Helpers;

namespace Ariana_Mcp.integrations.Services;

internal static class ArianaLabJson
{
    public static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNamingPolicy = null,
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
    };

    public static string Serialize(object value) =>
        JsonResponseCleaner.Clean(JsonSerializer.Serialize(value, SerializerOptions));
}
