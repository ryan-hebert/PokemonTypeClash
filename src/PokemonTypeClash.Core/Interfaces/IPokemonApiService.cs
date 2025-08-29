using System.Diagnostics.CodeAnalysis;
using PokemonTypeClash.Core.Models;

namespace PokemonTypeClash.Core.Interfaces;

/// <summary>
/// Service for retrieving Pokemon and type data from the PokéAPI
/// </summary>
public interface IPokemonApiService
{
    /// <summary>
    /// Retrieves Pokemon data by name or ID
    /// </summary>
    /// <param name="nameOrId">The Pokemon name or ID</param>
    /// <returns>The Pokemon data</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
    Task<Pokemon> GetPokemonAsync(string nameOrId);
    
    /// <summary>
    /// Retrieves type data by name or ID
    /// </summary>
    /// <param name="nameOrId">The type name or ID</param>
    /// <returns>The type data with effectiveness relationships</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
    Task<PokemonType> GetTypeAsync(string nameOrId);
}
