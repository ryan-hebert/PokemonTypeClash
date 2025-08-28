using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PokemonTypeClash.Application.Services;
using PokemonTypeClash.Core.Enums;
using PokemonTypeClash.Core.Interfaces;
using PokemonTypeClash.Core.Models;

namespace PokemonTypeClash.Application.Tests.Services;

public class TypeEffectivenessServiceTests
{
    private readonly Mock<ILogger<TypeEffectivenessService>> _loggerMock;
    private readonly Mock<ITypeDataService> _typeDataServiceMock;
    private readonly TypeEffectivenessService _service;

    public TypeEffectivenessServiceTests()
    {
        _loggerMock = new Mock<ILogger<TypeEffectivenessService>>();
        _typeDataServiceMock = new Mock<ITypeDataService>();
        _service = new TypeEffectivenessService(_loggerMock.Object, _typeDataServiceMock.Object);
    }

    [Fact]
    public async Task AnalyzeTypeEffectivenessAsync_WithSingleTypePokemon_ShouldReturnCorrectAnalysis()
    {
        // Arrange
        var electricType = CreateElectricType();
        var waterType = CreateWaterType();
        var groundType = CreateGroundType();
        
        var pikachu = new Pokemon
        {
            Id = 25,
            Name = "pikachu",
            Types = new List<PokemonType> { electricType }
        };

        var allTypes = new List<PokemonType> { electricType, waterType, groundType };
        _typeDataServiceMock.Setup(x => x.GetAllTypesAsync()).ReturnsAsync(allTypes);

        // Act
        var result = await _service.AnalyzeTypeEffectivenessAsync(pikachu);

        // Assert
        result.Should().NotBeNull();
        result.Pokemon.Should().Be(pikachu);
        result.AnalysisType.Should().Be(AnalysisType.Both);
        
        // Electric should be strong against water (2x damage)
        result.StrongAgainst.Should().Contain(t => t.Name == "water");
        
        // Electric should be weak against electric (0.5x damage)
        result.WeakAgainst.Should().Contain(t => t.Name == "electric");
        
        // Electric should be vulnerable to ground (2x damage from)
        result.VulnerableTo.Should().Contain(t => t.Name == "ground");
    }

    [Fact]
    public async Task AnalyzeTypeEffectivenessAsync_WithDualTypePokemon_ShouldReturnCorrectAnalysis()
    {
        // Arrange
        var fireType = CreateFireType();
        var flyingType = CreateFlyingType();
        var grassType = CreateGrassType();
        
        var charizard = new Pokemon
        {
            Id = 6,
            Name = "charizard",
            Types = new List<PokemonType> { fireType, flyingType }
        };

        var allTypes = new List<PokemonType> { fireType, flyingType, grassType };
        _typeDataServiceMock.Setup(x => x.GetAllTypesAsync()).ReturnsAsync(allTypes);

        // Act
        var result = await _service.AnalyzeTypeEffectivenessAsync(charizard);

        // Assert
        result.Should().NotBeNull();
        result.Pokemon.Should().Be(charizard);
        
        // Fire/Flying should be strong against grass (Fire 2x + Flying 2x = 4x total)
        result.StrongAgainst.Should().Contain(t => t.Name == "grass");
    }

    [Fact]
    public void CalculateEffectiveness_WithSuperEffective_ShouldReturnSuperEffective()
    {
        // Arrange
        var electricType = CreateElectricType();
        var waterType = CreateWaterType();

        // Act
        var result = _service.CalculateEffectiveness(electricType, waterType);

        // Assert
        result.Should().Be(TypeEffectiveness.SuperEffective);
    }

    [Fact]
    public void CalculateEffectiveness_WithNotVeryEffective_ShouldReturnNotVeryEffective()
    {
        // Arrange
        var electricType = CreateElectricType();
        var grassType = CreateGrassType();

        // Act
        var result = _service.CalculateEffectiveness(electricType, grassType);

        // Assert
        result.Should().Be(TypeEffectiveness.NotVeryEffective);
    }

    [Fact]
    public void CalculateEffectiveness_WithNoEffect_ShouldReturnNoEffect()
    {
        // Arrange
        var electricType = CreateElectricType();
        var groundType = CreateGroundType();

        // Act
        var result = _service.CalculateEffectiveness(electricType, groundType);

        // Assert
        result.Should().Be(TypeEffectiveness.NoEffect);
    }

    [Fact]
    public void CalculateEffectiveness_WithNormalEffectiveness_ShouldReturnNormal()
    {
        // Arrange
        var electricType = CreateElectricType();
        var fireType = CreateFireType();

        // Act
        var result = _service.CalculateEffectiveness(electricType, fireType);

        // Assert
        result.Should().Be(TypeEffectiveness.Normal);
    }





    [Fact]
    public async Task AnalyzeTypeEffectivenessAsync_WhenTypeDataServiceThrowsException_ShouldRethrow()
    {
        // Arrange
        var pokemon = new Pokemon
        {
            Id = 25,
            Name = "pikachu",
            Types = new List<PokemonType> { CreateElectricType() }
        };

        var expectedException = new InvalidOperationException("Type data service error");
        _typeDataServiceMock.Setup(x => x.GetAllTypesAsync())
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.AnalyzeTypeEffectivenessAsync(pokemon));

        exception.Should().Be(expectedException);
    }







    private static PokemonType CreateElectricType()
    {
        return new PokemonType
        {
            Id = 13,
            Name = "electric",
            Relations = new TypeRelations
            {
                DoubleDamageTo = new List<PokemonType> { new() { Id = 0, Name = "water" }, new() { Id = 0, Name = "flying" } },
                HalfDamageTo = new List<PokemonType> { new() { Id = 0, Name = "electric" }, new() { Id = 0, Name = "grass" } },
                NoDamageTo = new List<PokemonType> { new() { Id = 0, Name = "ground" } },
                DoubleDamageFrom = new List<PokemonType> { new() { Id = 0, Name = "ground" } },
                HalfDamageFrom = new List<PokemonType> { new() { Id = 0, Name = "flying" }, new() { Id = 0, Name = "steel" }, new() { Id = 0, Name = "electric" } },
                NoDamageFrom = new List<PokemonType>()
            }
        };
    }

    private static PokemonType CreateWaterType()
    {
        return new PokemonType
        {
            Id = 11,
            Name = "water",
            Relations = new TypeRelations
            {
                DoubleDamageTo = new List<PokemonType> { new() { Id = 0, Name = "fire" }, new() { Id = 0, Name = "ground" }, new() { Id = 0, Name = "rock" } },
                HalfDamageTo = new List<PokemonType> { new() { Id = 0, Name = "water" }, new() { Id = 0, Name = "grass" }, new() { Id = 0, Name = "dragon" } },
                NoDamageTo = new List<PokemonType>(),
                DoubleDamageFrom = new List<PokemonType> { new() { Id = 0, Name = "electric" }, new() { Id = 0, Name = "grass" } },
                HalfDamageFrom = new List<PokemonType> { new() { Id = 0, Name = "fire" }, new() { Id = 0, Name = "water" }, new() { Id = 0, Name = "ice" }, new() { Id = 0, Name = "steel" } },
                NoDamageFrom = new List<PokemonType>()
            }
        };
    }

    private static PokemonType CreateGroundType()
    {
        return new PokemonType
        {
            Id = 5,
            Name = "ground",
            Relations = new TypeRelations
            {
                DoubleDamageTo = new List<PokemonType> { new() { Id = 0, Name = "fire" }, new() { Id = 0, Name = "electric" }, new() { Id = 0, Name = "poison" }, new() { Id = 0, Name = "rock" }, new() { Id = 0, Name = "steel" } },
                HalfDamageTo = new List<PokemonType> { new() { Id = 0, Name = "grass" }, new() { Id = 0, Name = "bug" } },
                NoDamageTo = new List<PokemonType> { new() { Id = 0, Name = "flying" } },
                DoubleDamageFrom = new List<PokemonType> { new() { Id = 0, Name = "water" }, new() { Id = 0, Name = "grass" }, new() { Id = 0, Name = "ice" } },
                HalfDamageFrom = new List<PokemonType> { new() { Id = 0, Name = "poison" }, new() { Id = 0, Name = "rock" } },
                NoDamageFrom = new List<PokemonType> { new() { Id = 0, Name = "electric" } }
            }
        };
    }

    private static PokemonType CreateFireType()
    {
        return new PokemonType
        {
            Id = 10,
            Name = "fire",
            Relations = new TypeRelations
            {
                DoubleDamageTo = new List<PokemonType> { new() { Id = 0, Name = "grass" }, new() { Id = 0, Name = "ice" }, new() { Id = 0, Name = "bug" }, new() { Id = 0, Name = "steel" } },
                HalfDamageTo = new List<PokemonType> { new() { Id = 0, Name = "fire" }, new() { Id = 0, Name = "water" }, new() { Id = 0, Name = "rock" }, new() { Id = 0, Name = "dragon" } },
                NoDamageTo = new List<PokemonType>(),
                DoubleDamageFrom = new List<PokemonType> { new() { Id = 0, Name = "water" }, new() { Id = 0, Name = "ground" }, new() { Id = 0, Name = "rock" } },
                HalfDamageFrom = new List<PokemonType> { new() { Id = 0, Name = "fire" }, new() { Id = 0, Name = "grass" }, new() { Id = 0, Name = "ice" }, new() { Id = 0, Name = "bug" }, new() { Id = 0, Name = "steel" } },
                NoDamageFrom = new List<PokemonType>()
            }
        };
    }

    private static PokemonType CreateFlyingType()
    {
        return new PokemonType
        {
            Id = 3,
            Name = "flying",
            Relations = new TypeRelations
            {
                DoubleDamageTo = new List<PokemonType> { new() { Id = 0, Name = "grass" }, new() { Id = 0, Name = "fighting" }, new() { Id = 0, Name = "bug" } },
                HalfDamageTo = new List<PokemonType> { new() { Id = 0, Name = "electric" }, new() { Id = 0, Name = "rock" }, new() { Id = 0, Name = "steel" } },
                NoDamageTo = new List<PokemonType>(),
                DoubleDamageFrom = new List<PokemonType> { new() { Id = 0, Name = "electric" }, new() { Id = 0, Name = "ice" }, new() { Id = 0, Name = "rock" } },
                HalfDamageFrom = new List<PokemonType> { new() { Id = 0, Name = "grass" }, new() { Id = 0, Name = "fighting" }, new() { Id = 0, Name = "bug" } },
                NoDamageFrom = new List<PokemonType> { new() { Id = 0, Name = "ground" } }
            }
        };
    }

    private static PokemonType CreateGrassType()
    {
        return new PokemonType
        {
            Id = 12,
            Name = "grass",
            Relations = new TypeRelations
            {
                DoubleDamageTo = new List<PokemonType> { new() { Id = 0, Name = "water" }, new() { Id = 0, Name = "ground" }, new() { Id = 0, Name = "rock" } },
                HalfDamageTo = new List<PokemonType> { new() { Id = 0, Name = "fire" }, new() { Id = 0, Name = "grass" }, new() { Id = 0, Name = "poison" }, new() { Id = 0, Name = "flying" }, new() { Id = 0, Name = "bug" }, new() { Id = 0, Name = "dragon" }, new() { Id = 0, Name = "steel" } },
                NoDamageTo = new List<PokemonType>(),
                DoubleDamageFrom = new List<PokemonType> { new() { Id = 0, Name = "fire" }, new() { Id = 0, Name = "ice" }, new() { Id = 0, Name = "poison" }, new() { Id = 0, Name = "flying" }, new() { Id = 0, Name = "bug" } },
                HalfDamageFrom = new List<PokemonType> { new() { Id = 0, Name = "water" }, new() { Id = 0, Name = "grass" }, new() { Id = 0, Name = "electric" }, new() { Id = 0, Name = "ground" } },
                NoDamageFrom = new List<PokemonType>()
            }
        };
    }

    private static PokemonType CreateRockType()
    {
        return new PokemonType
        {
            Id = 6,
            Name = "rock",
            Relations = new TypeRelations
            {
                DoubleDamageTo = new List<PokemonType> { new() { Id = 0, Name = "fire" }, new() { Id = 0, Name = "ice" }, new() { Id = 0, Name = "flying" }, new() { Id = 0, Name = "bug" } },
                HalfDamageTo = new List<PokemonType> { new() { Id = 0, Name = "fighting" }, new() { Id = 0, Name = "ground" }, new() { Id = 0, Name = "steel" } },
                NoDamageTo = new List<PokemonType>(),
                DoubleDamageFrom = new List<PokemonType> { new() { Id = 0, Name = "water" }, new() { Id = 0, Name = "grass" }, new() { Id = 0, Name = "fighting" }, new() { Id = 0, Name = "ground" }, new() { Id = 0, Name = "steel" } },
                HalfDamageFrom = new List<PokemonType> { new() { Id = 0, Name = "normal" }, new() { Id = 0, Name = "fire" }, new() { Id = 0, Name = "poison" }, new() { Id = 0, Name = "flying" } },
                NoDamageFrom = new List<PokemonType>()
            }
        };
    }

    private static PokemonType CreateSteelType()
    {
        return new PokemonType
        {
            Id = 9,
            Name = "steel",
            Relations = new TypeRelations
            {
                DoubleDamageTo = new List<PokemonType> { new() { Id = 0, Name = "ice" }, new() { Id = 0, Name = "rock" }, new() { Id = 0, Name = "fairy" } },
                HalfDamageTo = new List<PokemonType> { new() { Id = 0, Name = "fire" }, new() { Id = 0, Name = "water" }, new() { Id = 0, Name = "electric" }, new() { Id = 0, Name = "steel" } },
                NoDamageTo = new List<PokemonType>(),
                DoubleDamageFrom = new List<PokemonType> { new() { Id = 0, Name = "fire" }, new() { Id = 0, Name = "fighting" }, new() { Id = 0, Name = "ground" } },
                HalfDamageFrom = new List<PokemonType> { new() { Id = 0, Name = "normal" }, new() { Id = 0, Name = "grass" }, new() { Id = 0, Name = "ice" }, new() { Id = 0, Name = "flying" }, new() { Id = 0, Name = "psychic" }, new() { Id = 0, Name = "bug" }, new() { Id = 0, Name = "rock" }, new() { Id = 0, Name = "dragon" }, new() { Id = 0, Name = "steel" }, new() { Id = 0, Name = "fairy" } },
                NoDamageFrom = new List<PokemonType> { new() { Id = 0, Name = "poison" } }
            }
        };
    }

    private static PokemonType CreateIceType()
    {
        return new PokemonType
        {
            Id = 15,
            Name = "ice",
            Relations = new TypeRelations
            {
                DoubleDamageTo = new List<PokemonType> { new() { Id = 0, Name = "grass" }, new() { Id = 0, Name = "ground" }, new() { Id = 0, Name = "flying" }, new() { Id = 0, Name = "dragon" } },
                HalfDamageTo = new List<PokemonType> { new() { Id = 0, Name = "fire" }, new() { Id = 0, Name = "water" }, new() { Id = 0, Name = "ice" }, new() { Id = 0, Name = "steel" } },
                NoDamageTo = new List<PokemonType>(),
                DoubleDamageFrom = new List<PokemonType> { new() { Id = 0, Name = "fire" }, new() { Id = 0, Name = "fighting" }, new() { Id = 0, Name = "rock" }, new() { Id = 0, Name = "steel" } },
                HalfDamageFrom = new List<PokemonType> { new() { Id = 0, Name = "ice" } },
                NoDamageFrom = new List<PokemonType>()
            }
        };
    }
}
