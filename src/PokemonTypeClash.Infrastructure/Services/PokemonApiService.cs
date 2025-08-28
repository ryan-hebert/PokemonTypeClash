using Microsoft.Extensions.Logging;
using PokemonTypeClash.Infrastructure.DTOs;
using PokemonTypeClash.Infrastructure.Mappers;
using PokemonTypeClash.Core.Interfaces;
using PokemonTypeClash.Core.Models;
using PokemonTypeClash.Infrastructure.Http;

namespace PokemonTypeClash.Infrastructure.Services;

/// <summary>
/// Implementation of the PokéAPI service for retrieving Pokemon and type data
/// </summary>
public class PokemonApiService : IPokemonApiService
{
    private readonly PokeApiHttpClient _httpClient;
    private readonly IPokemonMapper _pokemonMapper;
    private readonly ITypeMapper _typeMapper;
    private readonly ILogger<PokemonApiService> _logger;
    private readonly ICacheService<Pokemon> _pokemonCache;
    private readonly ICacheService<PokemonType> _typeCache;

    public PokemonApiService(
        PokeApiHttpClient httpClient,
        IPokemonMapper pokemonMapper,
        ITypeMapper typeMapper,
        ILogger<PokemonApiService> logger,
        ICacheService<Pokemon> pokemonCache,
        ICacheService<PokemonType> typeCache)
    {
        _httpClient = httpClient;
        _pokemonMapper = pokemonMapper;
        _typeMapper = typeMapper;
        _logger = logger;
        _pokemonCache = pokemonCache;
        _typeCache = typeCache;
    }

    /// <summary>
    /// Retrieves Pokemon data by name or ID
    /// </summary>
    /// <param name="nameOrId">The Pokemon name or ID</param>
    /// <returns>The Pokemon data</returns>
    public async Task<Pokemon> GetPokemonAsync(string nameOrId)
    {
        var pokemonKey = nameOrId.ToLowerInvariant();
        
        // Check cache first
        var cachedPokemon = _pokemonCache.Get(pokemonKey);
        if (cachedPokemon != null)
        {
            _logger.LogDebug("Retrieved Pokemon from cache: {Name}", cachedPokemon.Name);
            return cachedPokemon;
        }
        
        _logger.LogInformation("Retrieving Pokemon data for: {NameOrId}", nameOrId);
        
        try
        {
            // Get Pokemon data from API
            var pokemonResponse = await _httpClient.GetAsync<PokemonApiResponse>($"pokemon/{pokemonKey}");
            
            // Get type data for each type
            var types = new List<PokemonType>();
            foreach (var typeSlot in pokemonResponse.Types)
            {
                var typeName = typeSlot.Type.Name;
                var type = await GetTypeAsync(typeName);
                types.Add(type);
            }
            
            // Map to domain model
            var pokemon = _pokemonMapper.MapToDomain(pokemonResponse, types);
            
            // Cache the Pokemon
            _pokemonCache.Set(pokemonKey, pokemon);
            
            _logger.LogInformation("Successfully retrieved Pokemon: {Name} (ID: {Id})", pokemon.Name, pokemon.Id);
            return pokemon;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve Pokemon data for: {NameOrId}", nameOrId);
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
