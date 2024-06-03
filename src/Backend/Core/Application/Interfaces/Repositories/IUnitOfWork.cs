using Domain.Common;

namespace Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Returns a generic repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The generic repository for the specified entity type.</returns>
        IGenericRepository<T> Repository<T>() where T : BaseAuditableEntity;

        /// <summary>
        /// Saves changes and removes the specified cache keys.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="cacheKeys">The cache keys to remove.</param>
        /// <returns>The number of rows affected.</returns>
        Task<int> SaveAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys);

        /// <summary>
        /// Saves changes asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SaveChangesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Saves changes synchronously.
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Rolls back the changes.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Rollback();
    }
}
