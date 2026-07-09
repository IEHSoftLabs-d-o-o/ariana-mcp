using System.Text.Json;
using Ariana_Mcp.integrations.Exceptions;

namespace Ariana_Mcp.integrations.Services;

internal static class BatchLookupHelper
{
    public static void EnsureNonEmptyList(IReadOnlyList<string>? inputs, string parameterName = "ids")
    {
        if (inputs is null || inputs.Count == 0)
            throw new ArianaLabException($"{parameterName} muss mindestens einen Eintrag enthalten.");
    }

    public static Task<string> ExecuteAsync(
        IReadOnlyList<string> ids,
        string emptyIdError,
        Func<string, CancellationToken, Task<string>> lookup,
        CancellationToken cancellationToken)
        => ExecuteAsync(
            ids,
            "ids",
            emptyIdError,
            "id",
            lookup,
            cancellationToken);

    public static async Task<string> ExecuteAsync(
        IReadOnlyList<string> inputs,
        string parameterName,
        string emptyInputError,
        string resultKey,
        Func<string, CancellationToken, Task<string>> lookup,
        CancellationToken cancellationToken,
        IReadOnlyDictionary<string, object?>? envelopeExtras = null)
    {
        EnsureNonEmptyList(inputs, parameterName);

        var results = new List<Dictionary<string, object?>>(inputs.Count);

        foreach (var input in inputs)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(input))
            {
                results.Add(CreateResult(resultKey, input ?? string.Empty, false, error: emptyInputError));
                continue;
            }

            try
            {
                var data = await lookup(input, cancellationToken).ConfigureAwait(false);
                results.Add(CreateResult(
                    resultKey,
                    input,
                    true,
                    data: JsonSerializer.Deserialize<JsonElement>(data)));
            }
            catch (ArianaLabException ex)
            {
                results.Add(CreateResult(resultKey, input, false, error: ex.Message));
            }
        }

        var envelope = new Dictionary<string, object?>
        {
            ["requestedCount"] = inputs.Count,
            ["results"] = results,
        };

        if (envelopeExtras is not null)
        {
            foreach (var (key, value) in envelopeExtras)
                envelope[key] = value;
        }

        return JsonSerializer.Serialize(envelope);
    }

    private static Dictionary<string, object?> CreateResult(
        string resultKey,
        string input,
        bool success,
        JsonElement? data = null,
        string? error = null)
    {
        var result = new Dictionary<string, object?>
        {
            [resultKey] = input,
            ["success"] = success,
        };

        if (success && data is not null)
            result["data"] = data;
        else if (!success && error is not null)
            result["error"] = error;

        return result;
    }
}
