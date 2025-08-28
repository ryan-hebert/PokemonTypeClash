using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PokemonTypeClash.Core.Interfaces;
using PokemonTypeClash.Core.Models;
using PokemonTypeClash.Infrastructure.Configuration;
using PokemonTypeClash.Infrastructure.Http;
using PokemonTypeClash.Infrastructure.Mappers;
using PokemonTypeClash.Infrastructure.Services;

namespace PokemonTypeClash.Infrastructure.Tests.Integration;

public class PokemonApiServiceIntegrationTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IPokemonApiService _pokemonApiService;

    public PokemonApiServiceIntegrationTests()
    {
        var services = new ServiceCollection();
        
        // Configure logging
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Information);
        });
        
        // Configure services
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["PokeApi:BaseUrl"] = "https://pokeapi.co/api/v2/",
                ["PokeApi:TimeoutSeconds"] = "30",
                ["PokeApi:MaxRetries"] = "3",
                ["PokeApi:CacheDurationMinutes"] = "60"
            })
            .Build();
        
        services.AddInfrastructureServices(configuration);
        
        _serviceProvider = services.BuildServiceProvider();
        _pokemonApiService = _serviceProvider.GetRequiredService<IPokemonApiService>();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetPokemonAsync_WithValidName_ShouldReturnPokemon()
    {
        // Act
        var pokemon = await _pokemonApiService.GetPokemonAsync("pikachu");

        // Assert
        pokemon.Should().NotBeNull();
        pokemon.Name.Should().Be("pikachu");
        pokemon.Id.Should().Be(25);
        pokemon.Types.Should().HaveCount(1);
        pokemon.Types[0].Name.Should().Be("electric");
        pokemon.Types[0].Relations.Should().NotBeNull();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetPokemonAsync_WithValidId_ShouldReturnPokemon()
    {
        // Act
        var pokemon = await _pokemonApiService.GetPokemonAsync("25");

        // Assert
        pokemon.Should().NotBeNull();
        pokemon.Name.Should().Be("pikachu");
        pokemon.Id.Should().Be(25);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetPokemonAsync_WithDualTypePokemon_ShouldReturnBothTypes()
    {
        // Act
        var pokemon = await _pokemonApiService.GetPokemonAsync("charizard");

        // Assert
        pokemon.Should().NotBeNull();
        pokemon.Name.Should().Be("charizard");
        pokemon.Types.Should().HaveCount(2);
        pokemon.Types.Should().Contain(t => t.Name == "fire");
        pokemon.Types.Should().Contain(t => t.Name == "flying");
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetTypeAsync_WithValidName_ShouldReturnType()
    {
        // Act
        var type = await _pokemonApiService.GetTypeAsync("electric");

        // Assert
        type.Should().NotBeNull();
        type.Name.Should().Be("electric");
        type.Relations.Should().NotBeNull();
        type.Relations.DoubleDamageTo.Should().Contain(t => t.Name == "water");
        type.Relations.DoubleDamageTo.Should().Contain(t => t.Name == "flying");
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetTypeAsync_WithCaching_ShouldReturnCachedType()
    {
        // Act - First call
        var type1 = await _pokemonApiService.GetTypeAsync("fire");
        
        // Act - Second call (should be cached)
        var type2 = await _pokemonApiService.GetTypeAsync("fire");

        // Assert
        type1.Should().NotBeNull();
        type2.Should().NotBeNull();
        type1.Name.Should().Be("fire");
        type2.Name.Should().Be("fire");
        // Both should be the same instance due to caching
        ReferenceEquals(type1, type2).Should().BeTrue();
    }

    public void Dispose()
    {
        _serviceProvider?.Dispose();
    }
}
