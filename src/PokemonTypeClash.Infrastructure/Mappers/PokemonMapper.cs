using PokemonTypeClash.Infrastructure.DTOs;
using PokemonTypeClash.Core.Models;

namespace PokemonTypeClash.Infrastructure.Mappers;

/// <summary>
/// Implementation of Pokemon mapper for transforming API responses to domain models
/// </summary>
public class PokemonMapper : IPokemonMapper
{
    /// <summary>
    /// Maps a Pokemon API response to a domain Pokemon model
    /// </summary>
    /// <param name="apiResponse">The API response DTO</param>
    /// <param name="types">The resolved Pokemon types</param>
    /// <returns>The domain Pokemon model</returns>
    public Pokemon MapToDomain(PokemonApiResponse apiResponse, List<PokemonType> types)
    {
        return new Pokemon
        {
            Id = apiResponse.Id,
            Name = apiResponse.Name,
            Height = apiResponse.Height,
            Weight = apiResponse.Weight,
            Types = types.OrderBy(t => apiResponse.Types.FirstOrDefault(ts => ts.Type.Name == t.Name)?.Slot ?? 0).ToList()
        };
    }
}
