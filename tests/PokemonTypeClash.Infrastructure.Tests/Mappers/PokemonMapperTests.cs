using FluentAssertions;
using PokemonTypeClash.Core.Models;
using PokemonTypeClash.Infrastructure.DTOs;
using PokemonTypeClash.Infrastructure.Mappers;

namespace PokemonTypeClash.Infrastructure.Tests.Mappers;

public class PokemonMapperTests
{
    private readonly PokemonMapper _mapper;

    public PokemonMapperTests()
    {
        _mapper = new PokemonMapper();
    }

    [Fact]
    public void MapToDomain_WithValidResponse_ShouldMapCorrectly()
    {
        // Arrange
        var apiResponse = new PokemonApiResponse
        {
            Id = 25,
            Name = "pikachu",
            Height = 4,
            Weight = 60,
            Types = new List<PokemonTypeSlot>
            {
                new() { Slot = 1, Type = new TypeReference { Name = "electric", Url = "https://pokeapi.co/api/v2/type/13/" } }
            }
        };

        var types = new List<PokemonType>
        {
            new() { Id = 13, Name = "electric", Relations = new TypeRelations() }
        };

        // Act
        var result = _mapper.MapToDomain(apiResponse, types);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(25);
        result.Name.Should().Be("pikachu");
        result.Height.Should().Be(4);
        result.Weight.Should().Be(60);
        result.Types.Should().HaveCount(1);
        result.Types[0].Name.Should().Be("electric");
    }

    [Fact]
    public void MapToDomain_WithDualTypePokemon_ShouldOrderTypesBySlot()
    {
        // Arrange
        var apiResponse = new PokemonApiResponse
        {
            Id = 6,
            Name = "charizard",
            Height = 17,
            Weight = 905,
            Types = new List<PokemonTypeSlot>
            {
                new() { Slot = 1, Type = new TypeReference { Name = "fire", Url = "https://pokeapi.co/api/v2/type/10/" } },
                new() { Slot = 2, Type = new TypeReference { Name = "flying", Url = "https://pokeapi.co/api/v2/type/3/" } }
            }
        };

        var types = new List<PokemonType>
        {
            new() { Id = 3, Name = "flying", Relations = new TypeRelations() },
            new() { Id = 10, Name = "fire", Relations = new TypeRelations() }
        };

        // Act
        var result = _mapper.MapToDomain(apiResponse, types);

        // Assert
        result.Should().NotBeNull();
        result.Types.Should().HaveCount(2);
        result.Types[0].Name.Should().Be("fire"); // First slot
        result.Types[1].Name.Should().Be("flying"); // Second slot
    }
}
