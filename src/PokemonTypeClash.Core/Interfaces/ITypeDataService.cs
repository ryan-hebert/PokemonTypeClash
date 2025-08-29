using System.Diagnostics.CodeAnalysis;
using PokemonTypeClash.Core.Models;

namespace PokemonTypeClash.Core.Interfaces;

/// <summary>
/// Service for retrieving Pokemon type data
/// </summary>
public interface ITypeDataService
{
    /// <summary>
    /// Gets all available Pokemon types
    /// </summary>
    /// <returns>List of all Pokemon types</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
    Task<List<PokemonType>> GetAllTypesAsync();
    
    /// <summary>
    /// Gets a specific type by name
    /// </summary>
    /// <param name="typeName">The name of the type</param>
    /// <returns>The Pokemon type</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
    Task<PokemonType> GetTypeAsync(string typeName);
}
