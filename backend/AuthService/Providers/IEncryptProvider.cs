namespace AuthService.Providers;

/// <summary>
/// Provides functionalities to encrypt the string
/// </summary>
public interface IEncryptProvider
{
    /// <summary>
    /// Encrypts the plain text
    /// </summary>
    /// <param name="plainText">The plain text to encrypt</param>
    /// <returns>The encrypted string</returns>
    public string Encrypt(string plainText);

    /// <summary>
    /// Verifys the plain text with encrypted text
    /// </summary>
    /// <param name="plainText">The plain text to compare</param>
    /// <param name="encryptedText">The encrypted text to be compared</param>
    /// <returns></returns>
    public bool VerifyEncryptedText(string plainText, string encryptedText);
}
