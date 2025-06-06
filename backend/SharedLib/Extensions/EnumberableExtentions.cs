using System.Collections.Generic;
using System.Linq;

namespace SharedLib.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IEnumerable{T}"/>
/// </summary>
public static class EnumberableExtentions
{
    /// <summary>
    /// Determines whether the specified enumerable is <c>null</c> or contains no elements
    /// </summary>
    /// <param name="items">The enumerable to check</param>
    /// <returns><c>true</c> if the enumerable is <c>null</c> or empty; otherwise, <c>false</c></returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> items)
    {
        return items is null || !items.Any();
    }
}
