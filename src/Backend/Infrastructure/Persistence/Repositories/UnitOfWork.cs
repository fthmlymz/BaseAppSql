using Application.Interfaces.Repositories;
using Domain.Common;
using Persistence.Context;
using System.Collections;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private Hashtable _repositories;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the UnitOfWork class with the specified <paramref name="dbContext"/>.
        /// </summary>
        /// <param name="dbContext">The application database context.</param>
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Gets the repositories hash table.
        /// </summary>
        /// <returns>A <see cref="Hashtable"/> containing the repositories.</returns>
        private Hashtable Repositories
        {
            get
            {
                if (_repositories == null)
                    _repositories = new Hashtable();
                return _repositories;
            }
        }

        /// <summary>
        /// Gets the repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <returns>The repository for the specified entity type.</returns>
        public IGenericRepository<T> Repository<T>() where T : BaseAuditableEntity
        {
            var type = typeof(T).Name;

            if (!Repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _dbContext);
                Repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<T>)Repositories[type];
        }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Saves all changes made in this context to the database asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Rolls back the tracked changes in the database context.
        /// </summary>
        /// <returns>A task that represents the asynchronous rollback operation.</returns>
        public Task Rollback()
        {
            _dbContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            return Task.CompletedTask;
        }

        /// <summary>
        /// Saves changes and removes cache entries based on the specified cache keys.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <param name="cacheKeys">The keys of the cache entries to remove.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task<int> SaveAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the object and releases any associated resources.
        /// </summary>
        /// <param name="disposing">A boolean value indicating whether the method is being called from the Dispose method.</param>
        /// <returns>A task that represents the asynchronous dispose operation.</returns>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                _dbContext.Dispose();
                _repositories?.Clear();
            }

            disposed = true;
        }
    }
}