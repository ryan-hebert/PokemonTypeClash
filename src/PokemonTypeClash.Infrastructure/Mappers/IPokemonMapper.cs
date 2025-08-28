using PokemonTypeClash.Infrastructure.DTOs;
using PokemonTypeClash.Core.Models;

namespace PokemonTypeClash.Infrastructure.Mappers;

/// <summary>
/// Interface for mapping Pokemon API responses to domain models
/// </summary>
public interface IPokemonMapper
{
    /// <summary>
    /// Maps a Pokemon API response to a domain Pokemon model
    /// </summary>
    /// <param name="apiResponse">The API response DTO</param>
    /// <param name="types">The resolved Pokemon types</param>
    /// <returns>The domain Pokemon model</returns>
    Pokemon MapToDomain(PokemonApiResponse apiResponse, List<PokemonType> types);
}
