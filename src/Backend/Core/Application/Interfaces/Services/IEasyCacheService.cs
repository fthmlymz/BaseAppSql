namespace Application.Interfaces.Services
{
    /// <summary>
    /// Provides methods for interacting with an EasyCache service.
    /// </summary>
    public interface IEasyCacheService
    {
        /// <summary>
        /// Retrieves an object from the cache asynchronously.
        /// </summary>
        /// <param name="key">The key of the object to retrieve.</param>
        /// <param name="cachedData">The type of the cached data.</param>
        /// <returns>The retrieved object, or null if it does not exist.</returns>
        Task<object> GetAsync(string key, Type cachedData);

        /// <summary>
        /// Retrieves a strongly-typed object from the cache asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the object to retrieve.</typeparam>
        /// <param name="key">The key of the object to retrieve.</param>
        /// <returns>The retrieved object, or default(T) if it does not exist.</returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// Checks if an object exists in the cache asynchronously.
        /// </summary>
        /// <param name="key">The key of the object to check.</param>
        /// <returns>True if the object exists, false otherwise.</returns>
        Task<bool> GetAnyAsync(string key);

        /// <summary>
        /// Sets an object in the cache asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the object to set.</typeparam>
        /// <param name="key">The key of the object to set.</param>
        /// <param name="value">The value of the object to set.</param>
        /// <param name="expiration">The optional expiration time for the object.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);

        /// <summary>
        /// Removes an object from the cache asynchronously.
        /// </summary>
        /// <param name="key">The key of the object to remove.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task RemoveAsync(string key);

        /// <summary>
        /// Removes objects with the specified prefix from the cache asynchronously.
        /// </summary>
        /// <param name="prefix">The prefix of the objects to remove.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task RemoveByPrefixAsync(string prefix);
    }
}
