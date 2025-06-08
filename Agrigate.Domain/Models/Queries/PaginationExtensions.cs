using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Agrigate.Domain.Models.Queries;

/// <summary>
/// Pagination-related extensions
/// </summary>
public static class PaginationExtensions
{
    /// <summary>
    /// Takes an IQueryable and returns the paginated results
    /// </summary>
    /// <param name="items">The items to query</param>
    /// <param name="pageNumber">The page number to retrieve</param>
    /// <param name="itemsPerPage">The number of items to include per page</param>
    /// <param name="selector">A selector to determine which property to order by</param>
    /// <param name="requestUrl">The current request url</param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <returns></returns>
    public static async Task<PaginatedResult<T>> ToPaginatedResult<T, TKey>(
        this IQueryable<T> items,
        int pageNumber,
        int itemsPerPage,
        string requestUrl,
        Expression<Func<T, TKey>> selector,
        CancellationToken cancellationToken = default
    ) where T : class, new()
    {
        var totalItems = items.Count();
        var pageItems = await items
            .OrderBy(selector)
            .Skip((pageNumber - 1) * itemsPerPage)
            .Take(itemsPerPage)
            .ToListAsync(cancellationToken);

        return new PaginatedResult<T>(
            pageItems,
            totalItems,
            pageNumber,
            itemsPerPage,
            requestUrl
        );
    }
}