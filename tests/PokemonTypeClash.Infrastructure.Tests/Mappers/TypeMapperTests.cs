using FluentAssertions;
using PokemonTypeClash.Core.Models;
using PokemonTypeClash.Infrastructure.DTOs;
using PokemonTypeClash.Infrastructure.Mappers;

namespace PokemonTypeClash.Infrastructure.Tests.Mappers;

public class TypeMapperTests
{
    private readonly TypeMapper _mapper;

    public TypeMapperTests()
    {
        _mapper = new TypeMapper();
    }

    [Fact]
    public void MapToDomain_WithValidResponse_ShouldMapCorrectly()
    {
        // Arrange
        var apiResponse = new TypeApiResponse
        {
            Id = 13,
            Name = "electric",
            DamageRelations = new DamageRelations
            {
                DoubleDamageTo = new List<TypeReference> { new() { Name = "water" }, new() { Name = "flying" } },
                HalfDamageTo = new List<TypeReference> { new() { Name = "electric" }, new() { Name = "grass" } },
                NoDamageTo = new List<TypeReference> { new() { Name = "ground" } },
                DoubleDamageFrom = new List<TypeReference> { new() { Name = "ground" } },
                HalfDamageFrom = new List<TypeReference> { new() { Name = "flying" }, new() { Name = "steel" } },
                NoDamageFrom = new List<TypeReference>()
            }
        };

        // Act
        var result = _mapper.MapToDomain(apiResponse);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(13);
        result.Name.Should().Be("electric");
        result.Relations.Should().NotBeNull();
        
        result.Relations.DoubleDamageTo.Should().HaveCount(2);
        result.Relations.DoubleDamageTo.Should().Contain(t => t.Name == "water");
        result.Relations.DoubleDamageTo.Should().Contain(t => t.Name == "flying");
        
        result.Relations.HalfDamageTo.Should().HaveCount(2);
        result.Relations.HalfDamageTo.Should().Contain(t => t.Name == "electric");
        result.Relations.HalfDamageTo.Should().Contain(t => t.Name == "grass");
        
        result.Relations.NoDamageTo.Should().HaveCount(1);
        result.Relations.NoDamageTo.Should().Contain(t => t.Name == "ground");
        
        result.Relations.DoubleDamageFrom.Should().HaveCount(1);
        result.Relations.DoubleDamageFrom.Should().Contain(t => t.Name == "ground");
        
        result.Relations.HalfDamageFrom.Should().HaveCount(2);
        result.Relations.HalfDamageFrom.Should().Contain(t => t.Name == "flying");
        result.Relations.HalfDamageFrom.Should().Contain(t => t.Name == "steel");
        
        result.Relations.NoDamageFrom.Should().BeEmpty();
    }

    [Fact]
    public void MapToDomain_WithEmptyRelations_ShouldMapCorrectly()
    {
        // Arrange
        var apiResponse = new TypeApiResponse
        {
            Id = 1,
            Name = "normal",
            DamageRelations = new DamageRelations()
        };

        // Act
        var result = _mapper.MapToDomain(apiResponse);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Name.Should().Be("normal");
        result.Relations.Should().NotBeNull();
        result.Relations.DoubleDamageTo.Should().BeEmpty();
        result.Relations.HalfDamageTo.Should().BeEmpty();
        result.Relations.NoDamageTo.Should().BeEmpty();
        result.Relations.DoubleDamageFrom.Should().BeEmpty();
        result.Relations.HalfDamageFrom.Should().BeEmpty();
        result.Relations.NoDamageFrom.Should().BeEmpty();
    }
}
