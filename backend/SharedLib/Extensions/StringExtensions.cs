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
    public static bool IsNullOrEmpty(this string? value)
    {
        return string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Determines whether the two strings are same ignoring the case
    /// </summary>
    /// <param name="value">The first value to be compared</param>
    /// <param name="otherValue">The second value to be compared</param>
    /// <returns><c>true</c> if two strings are same; otherwise <c>false</c></returns>
    public static bool EqualsIgnoreCase(this string? value, string? otherValue)
    {
        return string.Equals(value, otherValue, System.StringComparison.OrdinalIgnoreCase);
    }
}
