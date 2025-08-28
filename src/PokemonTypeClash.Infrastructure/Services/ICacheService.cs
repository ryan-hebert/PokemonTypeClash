namespace PokemonTypeClash.Infrastructure.Services;

/// <summary>
/// Generic cache service interface for storing API results with expiration
/// </summary>
/// <typeparam name="T">The type of data to cache</typeparam>
public interface ICacheService<T> where T : class
{
    /// <summary>
    /// Gets an item from cache if it exists and is not expired
    /// </summary>
    /// <param name="key">The cache key</param>
    /// <returns>The cached item or null if not found or expired</returns>
    T? Get(string key);

    /// <summary>
    /// Adds an item to the cache with the configured expiration time
    /// </summary>
    /// <param name="key">The cache key</param>
    /// <param name="value">The value to cache</param>
    void Set(string key, T value);

    /// <summary>
    /// Removes an item from the cache
    /// </summary>
    /// <param name="key">The cache key</param>
    void Remove(string key);

    /// <summary>
    /// Clears all expired items from the cache
    /// </summary>
    void CleanupExpiredItems();

    /// <summary>
    /// Gets the current cache statistics
    /// </summary>
    /// <returns>Cache statistics</returns>
    CacheStatistics GetStatistics();
}
