using System.Net;

namespace Ariana_Mcp.integrations.Exceptions;

public sealed class ArianaLabException : Exception
{
    public ArianaLabException(string message, HttpStatusCode? statusCode = null, Exception? innerException = null)
        : base(message, innerException)
    {
        StatusCode = statusCode;
    }

    public HttpStatusCode? StatusCode { get; }
}
