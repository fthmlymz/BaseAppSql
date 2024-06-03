using Microsoft.EntityFrameworkCore;
using Shared;

namespace Application.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Converts an IQueryable to a paginated list of a specified type.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The IQueryable to convert.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The size of each page.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A Task representing the asynchronous operation. The task result contains a PaginatedResult of type T.</returns>
        public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken) where T : class
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : pageSize;
            bool anyRecords = await source.AnyAsync();

            if (!anyRecords)
            {
                return PaginatedResult<T>.Create(new List<T>(), 0, pageNumber, pageSize);
            }
            List<T> items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return PaginatedResult<T>.Create(items, items.Count, pageNumber, pageSize);
        }
    }
}
