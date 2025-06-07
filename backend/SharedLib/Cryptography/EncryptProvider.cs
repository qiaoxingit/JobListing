using SharedLib.Bootstrap;
using System;
using System.Composition;
using System.Security.Cryptography;
using System.Text;

namespace SharedLib.Cryptography;

/// <summary>
/// Provides functionalities to encrypt the string
/// </summary>
[Export(typeof(IEncryptProvider))]
public class EncryptProvider(AppSettings appSettings) : IEncryptProvider
{
    /// <inheritdoc />
    public string Encrypt(string plainText)
    {
        var fixedSalt = Encoding.UTF8.GetBytes(appSettings.FixedSalt);

        using var pbkdf2 = new Rfc2898DeriveBytes(plainText, fixedSalt, 100_000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(32);
        return Convert.ToBase64String(hash);
    }

    /// <inheritdoc />
    public bool VerifyEncryptedText(string plainText, string encryptedText)
    {
        string inputHash = Encrypt(plainText);
        return CryptographicOperations.FixedTimeEquals
        (
            Convert.FromBase64String(inputHash),
            Convert.FromBase64String(encryptedText)
        );
    }
}
