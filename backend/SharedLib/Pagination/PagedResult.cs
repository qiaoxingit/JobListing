using System.Collections.Generic;

namespace SharedLib.Pagination;

/// <summary>
/// Represents the paged result with total hits
/// </summary>
/// <typeparam name="T">The type of the item in the result list</typeparam>
/// <param name="items">The list of result in this page</param>
/// <param name="resultCount">The count of current page</param>
/// <param name="totalCount">The total hits</param>
public class PagedResult<T>
{
    /// <summary>
    /// Gets or sets the list of result in this page
    /// </summary>
    public required IReadOnlyList<T> Items { get; init; }

    /// <summary>
    /// Gets or sets the count of current page
    /// </summary>
    public int ResultCount { get; init; }

    /// <summary>
    /// Gets or sets the total hits
    /// </summary>
    public int TotalCount { get; init; }
}
