using PokemonTypeClash.Application.DTOs;
using PokemonTypeClash.Core.Models;

namespace PokemonTypeClash.Application.Mappers;

/// <summary>
/// Interface for mapping Type API responses to domain models
/// </summary>
public interface ITypeMapper
{
    /// <summary>
    /// Maps a Type API response to a domain PokemonType model
    /// </summary>
    /// <param name="apiResponse">The API response DTO</param>
    /// <returns>The domain PokemonType model</returns>
    PokemonType MapToDomain(TypeApiResponse apiResponse);
}
