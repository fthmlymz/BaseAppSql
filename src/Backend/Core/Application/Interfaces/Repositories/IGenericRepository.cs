using Domain.Common.Interfaces;
using System.Linq.Expressions;

namespace Application.Interfaces.Repositories
{
    /// <summary>
    /// Represents a generic repository for entities.
    /// </summary>
    /// <typeparam name="T">The type of entity being stored.</typeparam>
    public interface IGenericRepository<T> where T : class, IEntity
    {
        /// <summary>
        /// Gets the queryable collection of entities.
        /// </summary>
        IQueryable<T> Entities { get; }

        /// <summary>
        /// Retrieves an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The entity with the specified ID, or null if it does not exist.</returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Retrieves all entities.
        /// </summary>
        /// <returns>A list of all entities.</returns>
        Task<List<T>> GetAllAsync();

        /// <summary>
        /// Adds a new entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity.</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        Task DeleteAsync(T entity);


        /// <summary>
        /// Filters the entities based on the given expression.
        /// </summary>
        /// <param name="expression">The expression to filter the entities.</param>
        /// <returns>The filtered entities.</returns>
        IQueryable<T> Where(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Checks if any entity satisfies the given expression.
        /// </summary>
        /// <param name="expression">The expression to check for.</param>
        /// <returns>True if any entity satisfies the expression, false otherwise.</returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
    }
}
