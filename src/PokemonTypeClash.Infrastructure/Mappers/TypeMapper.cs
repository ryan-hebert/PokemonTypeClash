using PokemonTypeClash.Infrastructure.DTOs;
using PokemonTypeClash.Core.Models;

namespace PokemonTypeClash.Infrastructure.Mappers;

/// <summary>
/// Implementation of Type mapper for transforming API responses to domain models
/// </summary>
public class TypeMapper : ITypeMapper
{
    /// <summary>
    /// Maps a Type API response to a domain PokemonType model
    /// </summary>
    /// <param name="apiResponse">The API response DTO</param>
    /// <returns>The domain PokemonType model</returns>
    public PokemonType MapToDomain(TypeApiResponse apiResponse)
    {
        return new PokemonType
        {
            Id = apiResponse.Id,
            Name = apiResponse.Name,
            Relations = new TypeRelations
            {
                DoubleDamageTo = apiResponse.DamageRelations.DoubleDamageTo.Select(t => new PokemonType { Id = 0, Name = t.Name }).ToList(),
                HalfDamageTo = apiResponse.DamageRelations.HalfDamageTo.Select(t => new PokemonType { Id = 0, Name = t.Name }).ToList(),
                NoDamageTo = apiResponse.DamageRelations.NoDamageTo.Select(t => new PokemonType { Id = 0, Name = t.Name }).ToList(),
                DoubleDamageFrom = apiResponse.DamageRelations.DoubleDamageFrom.Select(t => new PokemonType { Id = 0, Name = t.Name }).ToList(),
                HalfDamageFrom = apiResponse.DamageRelations.HalfDamageFrom.Select(t => new PokemonType { Id = 0, Name = t.Name }).ToList(),
                NoDamageFrom = apiResponse.DamageRelations.NoDamageFrom.Select(t => new PokemonType { Id = 0, Name = t.Name }).ToList()
            }
        };
    }
}
