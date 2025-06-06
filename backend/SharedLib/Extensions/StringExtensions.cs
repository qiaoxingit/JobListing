namespace SharedLib.Extensions;

/// <summary>
/// Provides extension methods for <see cref="string"/>
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Determines whether the specified string is <c>null</c> or an empty string
    /// </summary>
    /// <param name="value">The string to test</param>
    /// <returns><c>true</c> if the string is null or empty; otherwise, <c>false</c></returns>
    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }
}
