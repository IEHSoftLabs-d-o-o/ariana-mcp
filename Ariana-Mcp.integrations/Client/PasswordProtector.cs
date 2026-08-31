using System.Security.Cryptography;
using System.Text;

namespace Ariana_Mcp.Integrations.AraianLab;

internal static class PasswordProtector
{
    private const int NonceSize = 12;
    private const int TagSize = 16;

    public static string Encrypt(string password, byte[] key)
    {
        var plaintext = Encoding.UTF8.GetBytes(password);
        var nonce = RandomNumberGenerator.GetBytes(NonceSize);
        var ciphertext = new byte[plaintext.Length];
        var tag = new byte[TagSize];

        using var aes = new AesGcm(key, TagSize);
        aes.Encrypt(nonce, plaintext, ciphertext, tag);

        var packed = new byte[NonceSize + TagSize + ciphertext.Length];
        Buffer.BlockCopy(nonce, 0, packed, 0, NonceSize);
        Buffer.BlockCopy(tag, 0, packed, NonceSize, TagSize);
        Buffer.BlockCopy(ciphertext, 0, packed, NonceSize + TagSize, ciphertext.Length);
        return Convert.ToBase64String(packed);
    }

    public static string Decrypt(string protectedPassword, byte[] key)
    {
        var packed = Convert.FromBase64String(protectedPassword);
        if (packed.Length < NonceSize + TagSize + 1)
            throw new FormatException("Protected password is invalid.");

        var nonce = packed.AsSpan(0, NonceSize);
        var tag = packed.AsSpan(NonceSize, TagSize);
        var ciphertext = packed.AsSpan(NonceSize + TagSize);
        var plaintext = new byte[ciphertext.Length];

        using var aes = new AesGcm(key, TagSize);
        aes.Decrypt(nonce, ciphertext, tag, plaintext);
        return Encoding.UTF8.GetString(plaintext);
    }
}
