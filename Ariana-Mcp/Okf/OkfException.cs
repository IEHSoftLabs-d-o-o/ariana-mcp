namespace Ariana_Mcp.Okf;

public sealed class OkfException : Exception
{
    public OkfException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
