using Microsoft.Extensions.Logging;
using PokemonTypeClash.Infrastructure.Configuration;

namespace PokemonTypeClash.Infrastructure.Services;

/// <summary>
/// Generic cache service for storing API results with expiration
/// </summary>
/// <typeparam name="T">The type of data to cache</typeparam>
public class CacheService<T> : ICacheService<T> where T : class
{
    private readonly Dictionary<string, CacheItem<T>> _cache = new();
    private readonly ILogger<CacheService<T>> _logger;
    private readonly PokeApiConfiguration _configuration;
    private readonly object _lockObject = new();

    public CacheService(ILogger<CacheService<T>> logger, PokeApiConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Gets an item from cache if it exists and is not expired
    /// </summary>
    /// <param name="key">The cache key</param>
    /// <returns>The cached item or null if not found or expired</returns>
    public T? Get(string key)
    {
        lock (_lockObject)
        {
            if (_cache.TryGetValue(key, out var cacheItem))
            {
                if (cacheItem.IsExpired)
                {
                    _logger.LogDebug("Cache item expired for key: {Key}", key);
                    _cache.Remove(key);
                    return null;
                }

                _logger.LogDebug("Cache hit for key: {Key}", key);
                return cacheItem.Value;
            }

            _logger.LogDebug("Cache miss for key: {Key}", key);
            return null;
        }
    }

    /// <summary>
    /// Adds an item to the cache with the configured expiration time
    /// </summary>
    /// <param name="key">The cache key</param>
    /// <param name="value">The value to cache</param>
    public void Set(string key, T value)
    {
        var expirationTime = DateTime.UtcNow.AddMinutes(_configuration.CacheDurationMinutes);
        var cacheItem = new CacheItem<T>(value, expirationTime);

        lock (_lockObject)
        {
            _cache[key] = cacheItem;
            _logger.LogDebug("Cached item for key: {Key}, expires at: {ExpirationTime}", key, expirationTime);
        }
    }

    /// <summary>
    /// Removes an item from the cache
    /// </summary>
    /// <param name="key">The cache key</param>
    public void Remove(string key)
    {
        lock (_lockObject)
        {
            if (_cache.Remove(key))
            {
                _logger.LogDebug("Removed cache item for key: {Key}", key);
            }
        }
    }

    /// <summary>
    /// Clears all expired items from the cache
    /// </summary>
    public void CleanupExpiredItems()
    {
        lock (_lockObject)
        {
            var expiredKeys = _cache.Where(kvp => kvp.Value.IsExpired).Select(kvp => kvp.Key).ToList();
            
            foreach (var key in expiredKeys)
            {
                _cache.Remove(key);
            }

            if (expiredKeys.Count > 0)
            {
                _logger.LogDebug("Cleaned up {Count} expired cache items", expiredKeys.Count);
            }
        }
    }

    /// <summary>
    /// Gets the current cache statistics
    /// </summary>
    /// <returns>Cache statistics</returns>
    public CacheStatistics GetStatistics()
    {
        lock (_lockObject)
        {
            var totalItems = _cache.Count;
            var expiredItems = _cache.Count(kvp => kvp.Value.IsExpired);
            var validItems = totalItems - expiredItems;

            return new CacheStatistics
            {
                TotalItems = totalItems,
                ValidItems = validItems,
                ExpiredItems = expiredItems
            };
        }
    }

    /// <summary>
    /// Represents a cached item with expiration time
    /// </summary>
    private class CacheItem<TValue>
    {
        public TValue Value { get; }
        public DateTime ExpirationTime { get; }
        public bool IsExpired => DateTime.UtcNow > ExpirationTime;

        public CacheItem(TValue value, DateTime expirationTime)
        {
            Value = value;
            ExpirationTime = expirationTime;
        }
    }
}

/// <summary>
/// Statistics about the cache
/// </summary>
public class CacheStatistics
{
    public int TotalItems { get; set; }
    public int ValidItems { get; set; }
    public int ExpiredItems { get; set; }
}
