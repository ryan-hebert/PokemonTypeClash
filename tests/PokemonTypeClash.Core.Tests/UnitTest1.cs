using FluentAssertions;
using PokemonTypeClash.Core.Enums;
using PokemonTypeClash.Core.Exceptions;
using PokemonTypeClash.Core.Models;

namespace PokemonTypeClash.Core.Tests;

public class CoreDomainTests
{
    [Fact]
    public void Pokemon_WithValidData_ShouldBeCreatedCorrectly()
    {
        // Arrange & Act
        var pokemon = new Pokemon
        {
            Id = 25,
            Name = "pikachu",
            Height = 4,
            Weight = 60,
            Types = new List<PokemonType>
            {
                new() { Id = 13, Name = "electric" }
            }
        };

        // Assert
        pokemon.Id.Should().Be(25);
        pokemon.Name.Should().Be("pikachu");
        pokemon.Height.Should().Be(4);
        pokemon.Weight.Should().Be(60);
        pokemon.Types.Should().HaveCount(1);
        pokemon.Types[0].Name.Should().Be("electric");
    }

    [Fact]
    public void PokemonType_WithValidData_ShouldBeCreatedCorrectly()
    {
        // Arrange & Act
        var type = new PokemonType
        {
            Id = 13,
            Name = "electric",
            Relations = new TypeRelations()
        };

        // Assert
        type.Id.Should().Be(13);
        type.Name.Should().Be("electric");
        type.Relations.Should().NotBeNull();
    }

    [Fact]
    public void TypeRelations_WithValidData_ShouldBeCreatedCorrectly()
    {
        // Arrange & Act
        var relations = new TypeRelations
        {
            DoubleDamageTo = new List<PokemonType> { new() { Name = "water" } },
            HalfDamageTo = new List<PokemonType> { new() { Name = "grass" } },
            NoDamageTo = new List<PokemonType> { new() { Name = "ground" } },
            DoubleDamageFrom = new List<PokemonType> { new() { Name = "ground" } },
            HalfDamageFrom = new List<PokemonType> { new() { Name = "flying" } },
            NoDamageFrom = new List<PokemonType>()
        };

        // Assert
        relations.DoubleDamageTo.Should().HaveCount(1);
        relations.DoubleDamageTo[0].Name.Should().Be("water");
        relations.HalfDamageTo.Should().HaveCount(1);
        relations.HalfDamageTo[0].Name.Should().Be("grass");
        relations.NoDamageTo.Should().HaveCount(1);
        relations.NoDamageTo[0].Name.Should().Be("ground");
        relations.DoubleDamageFrom.Should().HaveCount(1);
        relations.DoubleDamageFrom[0].Name.Should().Be("ground");
        relations.HalfDamageFrom.Should().HaveCount(1);
        relations.HalfDamageFrom[0].Name.Should().Be("flying");
        relations.NoDamageFrom.Should().BeEmpty();
    }

    [Fact]
    public void TypeAnalysisResult_WithValidData_ShouldBeCreatedCorrectly()
    {
        // Arrange
        var pokemon = new Pokemon { Id = 25, Name = "pikachu" };
        var strongType = new PokemonType { Name = "water" };
        var weakType = new PokemonType { Name = "grass" };

        // Act
        var result = new TypeAnalysisResult
        {
            Pokemon = pokemon,
            StrongAgainst = new List<PokemonType> { strongType },
            WeakAgainst = new List<PokemonType> { weakType },
            AnalysisType = AnalysisType.Both
        };

        // Assert
        result.Pokemon.Should().Be(pokemon);
        result.StrongAgainst.Should().HaveCount(1);
        result.StrongAgainst[0].Name.Should().Be("water");
        result.WeakAgainst.Should().HaveCount(1);
        result.WeakAgainst[0].Name.Should().Be("grass");
        result.AnalysisType.Should().Be(AnalysisType.Both);
        result.AnalysisTimestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void TypeEffectiveness_EnumValues_ShouldBeCorrect()
    {
        // Assert
        ((int)TypeEffectiveness.NoEffect).Should().Be(0);
        ((int)TypeEffectiveness.NotVeryEffective).Should().Be(1);
        ((int)TypeEffectiveness.Normal).Should().Be(2);
        ((int)TypeEffectiveness.SuperEffective).Should().Be(3);
    }

    [Fact]
    public void AnalysisType_EnumValues_ShouldBeCorrect()
    {
        // Assert
        ((int)AnalysisType.Offensive).Should().Be(0);
        ((int)AnalysisType.Defensive).Should().Be(1);
        ((int)AnalysisType.Both).Should().Be(2);
    }

    [Fact]
    public void PokemonApiException_WithMessage_ShouldBeCreatedCorrectly()
    {
        // Arrange & Act
        var exception = new PokemonApiException("API communication failed");

        // Assert
        exception.Message.Should().Be("API communication failed");
    }

    [Fact]
    public void PokemonApiException_WithInnerException_ShouldBeCreatedCorrectly()
    {
        // Arrange
        var innerException = new HttpRequestException("Network error");

        // Act
        var exception = new PokemonApiException("API communication failed", innerException);

        // Assert
        exception.Message.Should().Be("API communication failed");
        exception.InnerException.Should().Be(innerException);
    }
}
