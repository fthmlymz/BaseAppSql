namespace Application.Interfaces
{
    public interface ICacheable
    {
        /// <summary>
        /// Gets a value indicating whether to bypass the cache.
        /// </summary>
        /// <returns>True if bypassing the cache, false otherwise.</returns>
        bool BypassCache { get; }

        /// <summary>
        /// Gets the cache key as a string.
        /// </summary>
        /// <returns>The cache key as a string.</returns>
        string CacheKey { get; }
    }
}
