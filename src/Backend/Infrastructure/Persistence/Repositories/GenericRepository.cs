using Application.Interfaces.Repositories;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Linq.Expressions;

namespace Persistence.Repositories
{
    /// <summary>
    /// Represents a generic repository for entities that implement BaseAuditableEntity.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseAuditableEntity
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<T> _entities;

        /// <summary>
        /// Initializes a new instance of the GenericRepository class.
        /// </summary>
        /// <param name="dbContext">The application database context.</param>
        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _entities = _dbContext.Set<T>();
        }

        /// <summary>
        /// Gets the entities in the repository.
        /// </summary>
        public IQueryable<T> Entities => _entities;

        /// <summary>
        /// Adds an entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity.</returns>
        public async Task<T> AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
            return entity;
        }

        /// <summary>
        /// Updates an entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _dbContext.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            await Task.CompletedTask;
        }

        /// <summary>
        /// Deletes an entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteAsync(T entity)
        {
            _entities.RemoveRange(entity);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Gets all entities asynchronously.
        /// </summary>
        /// <returns>A list of entities.</returns>
        public async Task<List<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        /// <summary>
        /// Gets an entity by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The entity with the specified ID.</returns>
        public async Task<T> GetByIdAsync(int id)
        {
            return await _entities.FindAsync(id);
        }

        /// <summary>
        /// Filters entities based on a predicate.
        /// </summary>
        /// <param name="expression">The predicate to filter entities.</param>
        /// <returns>The filtered entities.</returns>
        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _entities.Where(expression);
        }

        /// <summary>
        /// Checks if any entities satisfy a given condition.
        /// </summary>
        /// <param name="expression">The condition to check.</param>
        /// <returns>True if any entity satisfies the condition, false otherwise.</returns>
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            //latest added
            if (expression == null)
            {
                return false;
            }

            return await _entities.AnyAsync(expression);
        }
    }
}
