using Microsoft.EntityFrameworkCore;

namespace Agrigate.Api.Core.Queries;

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
    /// <param name="requestUrl">The current request url</param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<PaginatedResult<T>> ToPaginatedResult<T>(
        this IQueryable<T> items,
        int pageNumber,
        int itemsPerPage,
        string requestUrl,
        CancellationToken cancellationToken = default
    ) where T : class, new()
    {
        var totalItems = items.Count();
        var pageItems = await items
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