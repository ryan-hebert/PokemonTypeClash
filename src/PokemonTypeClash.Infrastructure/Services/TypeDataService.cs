using Microsoft.Extensions.Logging;
using PokemonTypeClash.Infrastructure.DTOs;
using PokemonTypeClash.Infrastructure.Mappers;
using PokemonTypeClash.Core.Interfaces;
using PokemonTypeClash.Core.Models;
using PokemonTypeClash.Infrastructure.Http;

namespace PokemonTypeClash.Infrastructure.Services;

/// <summary>
/// Service for retrieving Pokemon type data
/// </summary>
public class TypeDataService : ITypeDataService
{
    private readonly IPokeApiHttpClient _httpClient;
    private readonly ITypeMapper _typeMapper;
    private readonly ILogger<TypeDataService> _logger;
    private readonly ICacheService<PokemonType> _typeCache;
    private readonly ICacheService<List<PokemonType>> _allTypesCache;

    public TypeDataService(
        IPokeApiHttpClient httpClient,
        ITypeMapper typeMapper,
        ILogger<TypeDataService> logger,
        ICacheService<PokemonType> typeCache,
        ICacheService<List<PokemonType>> allTypesCache)
    {
        _httpClient = httpClient;
        _typeMapper = typeMapper;
        _logger = logger;
        _typeCache = typeCache;
        _allTypesCache = allTypesCache;
    }

    /// <summary>
    /// Retrieves all Pokemon types
    /// </summary>
    /// <returns>List of all Pokemon types</returns>
    public async Task<List<PokemonType>> GetAllTypesAsync()
    {
        const string allTypesKey = "all_types";
        
        // Check cache first
        var cachedAllTypes = _allTypesCache.Get(allTypesKey);
        if (cachedAllTypes != null)
        {
            _logger.LogDebug("Retrieved all types from cache");
            return cachedAllTypes;
        }
        
        _logger.LogInformation("Retrieving all Pokemon types");
        
        try
        {
            var allTypes = new List<PokemonType>();
            var typeNames = new[]
            {
                "normal", "fighting", "flying", "poison", "ground", "rock",
                "bug", "ghost", "steel", "fire", "water", "grass",
                "electric", "psychic", "ice", "dragon", "dark", "fairy"
            };

            foreach (var typeName in typeNames)
            {
                // Check cache first
                var cachedType = _typeCache.Get(typeName);
                if (cachedType != null)
                {
                    allTypes.Add(cachedType);
                    continue;
                }

                // Get type data from API
                var typeResponse = await _httpClient.GetAsync<TypeApiResponse>($"type/{typeName}");
                var type = _typeMapper.MapToDomain(typeResponse);
                
                // Cache the type
                _typeCache.Set(typeName, type);
                allTypes.Add(type);
            }

            // Cache the complete list
            _allTypesCache.Set(allTypesKey, allTypes);
            
            _logger.LogInformation("Successfully retrieved all {Count} Pokemon types", allTypes.Count);
            return allTypes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve all Pokemon types");
            throw;
        }
    }

    /// <summary>
    /// Retrieves type data by name or ID
    /// </summary>
    /// <param name="nameOrId">The type name or ID</param>
    /// <returns>The type data with effectiveness relationships</returns>
    public async Task<PokemonType> GetTypeAsync(string nameOrId)
    {
        var typeKey = nameOrId.ToLowerInvariant();
        
        // Check cache first
        var cachedType = _typeCache.Get(typeKey);
        if (cachedType != null)
        {
            _logger.LogDebug("Retrieved type from cache: {TypeName}", cachedType.Name);
            return cachedType;
        }
        
        _logger.LogInformation("Retrieving type data for: {NameOrId}", nameOrId);
        
        try
        {
            // Get type data from API
            var typeResponse = await _httpClient.GetAsync<TypeApiResponse>($"type/{typeKey}");
            
            // Map to domain model
            var type = _typeMapper.MapToDomain(typeResponse);
            
            // Cache the type
            _typeCache.Set(typeKey, type);
            
            _logger.LogInformation("Successfully retrieved type: {Name} (ID: {Id})", type.Name, type.Id);
            return type;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve type data for: {NameOrId}", nameOrId);
            throw;
        }
    }
}

/// <summary>
/// DTO for the type list API response
/// </summary>
public class TypeListResponse
{
    public int Count { get; set; }
    public string? Next { get; set; }
    public string? Previous { get; set; }
    public List<TypeReference> Results { get; set; } = new();
}
