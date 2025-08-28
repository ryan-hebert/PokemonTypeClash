using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PokemonTypeClash.Core.Models;
using PokemonTypeClash.Infrastructure.Configuration;
using PokemonTypeClash.Infrastructure.DTOs;
using PokemonTypeClash.Infrastructure.Http;
using PokemonTypeClash.Infrastructure.Mappers;
using PokemonTypeClash.Infrastructure.Services;
using PokemonTypeClash.Core.Interfaces;

namespace PokemonTypeClash.Infrastructure.Tests;

public class TypeDataServiceTests
{
    private readonly Mock<IPokeApiHttpClient> _httpClientMock;
    private readonly Mock<ILogger<TypeDataService>> _loggerMock;
    private readonly Mock<ITypeMapper> _typeMapperMock;
    private readonly Mock<ICacheService<PokemonType>> _typeCacheMock;
    private readonly Mock<ICacheService<List<PokemonType>>> _allTypesCacheMock;
    private readonly PokeApiConfiguration _configuration;
    private readonly TypeDataService _service;

    public TypeDataServiceTests()
    {
        _httpClientMock = new Mock<IPokeApiHttpClient>();
        _loggerMock = new Mock<ILogger<TypeDataService>>();
        _typeMapperMock = new Mock<ITypeMapper>();
        _typeCacheMock = new Mock<ICacheService<PokemonType>>();
        _allTypesCacheMock = new Mock<ICacheService<List<PokemonType>>>();
        
        _configuration = new PokeApiConfiguration
        {
            CacheDurationMinutes = 60
        };
        
        _service = new TypeDataService(
            _httpClientMock.Object, 
            _typeMapperMock.Object,
            _loggerMock.Object,
            _typeCacheMock.Object,
            _allTypesCacheMock.Object);
    }

    [Fact]
    public async Task GetTypeAsync_WithValidTypeName_ShouldReturnType()
    {
        // Arrange
        var typeName = "electric";
        var apiResponse = new TypeApiResponse
        {
            Id = 13,
            Name = "electric",
            DamageRelations = new DamageRelations
            {
                DoubleDamageTo = new List<TypeReference> { new() { Name = "water" } },
                HalfDamageTo = new List<TypeReference> { new() { Name = "grass" } },
                NoDamageTo = new List<TypeReference>(),
                DoubleDamageFrom = new List<TypeReference> { new() { Name = "ground" } },
                HalfDamageFrom = new List<TypeReference>(),
                NoDamageFrom = new List<TypeReference>()
            }
        };

        var expectedType = new PokemonType
        {
            Id = 13,
            Name = "electric",
            Relations = new TypeRelations
            {
                DoubleDamageTo = new List<PokemonType> { new() { Name = "water" } },
                HalfDamageTo = new List<PokemonType> { new() { Name = "grass" } },
                NoDamageTo = new List<PokemonType>(),
                DoubleDamageFrom = new List<PokemonType> { new() { Name = "ground" } },
                HalfDamageFrom = new List<PokemonType>(),
                NoDamageFrom = new List<PokemonType>()
            }
        };

        // Setup cache to return null (cache miss)
        _typeCacheMock.Setup(x => x.Get(typeName))
            .Returns((PokemonType?)null);

        _httpClientMock.Setup(x => x.GetAsync<TypeApiResponse>($"type/{typeName}"))
            .ReturnsAsync(apiResponse);

        _typeMapperMock.Setup(x => x.MapToDomain(apiResponse))
            .Returns(expectedType);

        // Act
        var result = await _service.GetTypeAsync(typeName);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(13);
        result.Name.Should().Be("electric");
        result.Relations.Should().NotBeNull();
        result.Relations.DoubleDamageTo.Should().HaveCount(1);
        result.Relations.DoubleDamageTo[0].Name.Should().Be("water");
        result.Relations.HalfDamageTo.Should().HaveCount(1);
        result.Relations.HalfDamageTo[0].Name.Should().Be("grass");
        result.Relations.DoubleDamageFrom.Should().HaveCount(1);
        result.Relations.DoubleDamageFrom[0].Name.Should().Be("ground");
        
        // Verify cache was used
        _typeCacheMock.Verify(x => x.Get(typeName), Times.Once);
        _typeCacheMock.Verify(x => x.Set(typeName, expectedType), Times.Once);
    }

    [Fact]
    public async Task GetTypeAsync_WithCachedType_ShouldReturnCachedType()
    {
        // Arrange
        var typeName = "fire";
        var cachedType = new PokemonType
        {
            Id = 10,
            Name = "fire",
            Relations = new TypeRelations()
        };

        // Setup cache to return cached type on second call
        _typeCacheMock.SetupSequence(x => x.Get(typeName))
            .Returns((PokemonType?)null)  // First call: cache miss
            .Returns(cachedType);         // Second call: cache hit

        var apiResponse = new TypeApiResponse
        {
            Id = 10,
            Name = "fire",
            DamageRelations = new DamageRelations()
        };

        _httpClientMock.Setup(x => x.GetAsync<TypeApiResponse>($"type/{typeName}"))
            .ReturnsAsync(apiResponse);

        _typeMapperMock.Setup(x => x.MapToDomain(apiResponse))
            .Returns(cachedType);

        // Act - First call
        var result1 = await _service.GetTypeAsync(typeName);
        
        // Act - Second call (should be cached)
        var result2 = await _service.GetTypeAsync(typeName);

        // Assert
        result1.Should().NotBeNull();
        result2.Should().NotBeNull();
        result1.Name.Should().Be("fire");
        result2.Name.Should().Be("fire");
        
        // Verify HTTP client was only called once
        _httpClientMock.Verify(x => x.GetAsync<TypeApiResponse>($"type/{typeName}"), Times.Once);
        
        // Verify cache was used
        _typeCacheMock.Verify(x => x.Get(typeName), Times.Exactly(2));
        _typeCacheMock.Verify(x => x.Set(typeName, cachedType), Times.Once);
    }

    [Fact]
    public async Task GetTypeAsync_WithCaseInsensitiveName_ShouldReturnType()
    {
        // Arrange
        var typeName = "ELECTRIC";
        var apiResponse = new TypeApiResponse
        {
            Id = 13,
            Name = "electric",
            DamageRelations = new DamageRelations()
        };

        var expectedType = new PokemonType
        {
            Id = 13,
            Name = "electric",
            Relations = new TypeRelations()
        };

        // Setup cache to return null (cache miss)
        _typeCacheMock.Setup(x => x.Get("electric"))
            .Returns((PokemonType?)null);

        _httpClientMock.Setup(x => x.GetAsync<TypeApiResponse>("type/electric"))
            .ReturnsAsync(apiResponse);

        _typeMapperMock.Setup(x => x.MapToDomain(apiResponse))
            .Returns(expectedType);

        // Act
        var result = await _service.GetTypeAsync(typeName);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("electric");
        
        // Verify cache was used with lowercase key
        _typeCacheMock.Verify(x => x.Get("electric"), Times.Once);
        _typeCacheMock.Verify(x => x.Set("electric", expectedType), Times.Once);
    }

    [Fact]
    public async Task GetTypeAsync_WhenHttpClientThrowsException_ShouldRethrow()
    {
        // Arrange
        var typeName = "invalid";
        var expectedException = new HttpRequestException("API error");

        // Setup cache to return null (cache miss)
        _typeCacheMock.Setup(x => x.Get(typeName))
            .Returns((PokemonType?)null);

        _httpClientMock.Setup(x => x.GetAsync<TypeApiResponse>($"type/{typeName}"))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<HttpRequestException>(() => 
            _service.GetTypeAsync(typeName));
        
        exception.Should().Be(expectedException);
        
        // Verify cache was checked
        _typeCacheMock.Verify(x => x.Get(typeName), Times.Once);
    }

    [Fact]
    public async Task GetAllTypesAsync_WithValidResponse_ShouldReturnAllTypes()
    {
        // Arrange
        var fireTypeResponse = new TypeApiResponse
        {
            Id = 10,
            Name = "fire",
            DamageRelations = new DamageRelations()
        };

        var waterTypeResponse = new TypeApiResponse
        {
            Id = 11,
            Name = "water",
            DamageRelations = new DamageRelations()
        };

        var fireType = new PokemonType { Id = 10, Name = "fire", Relations = new TypeRelations() };
        var waterType = new PokemonType { Id = 11, Name = "water", Relations = new TypeRelations() };
        var allTypes = new List<PokemonType> { fireType, waterType };

        // Setup cache to return null (cache miss)
        _allTypesCacheMock.Setup(x => x.Get("all_types"))
            .Returns((List<PokemonType>?)null);

        // Setup individual type caches to return null (cache miss)
        _typeCacheMock.Setup(x => x.Get("fire"))
            .Returns((PokemonType?)null);
        _typeCacheMock.Setup(x => x.Get("water"))
            .Returns((PokemonType?)null);

        _httpClientMock.Setup(x => x.GetAsync<TypeApiResponse>("type/fire"))
            .ReturnsAsync(fireTypeResponse);
        _httpClientMock.Setup(x => x.GetAsync<TypeApiResponse>("type/water"))
            .ReturnsAsync(waterTypeResponse);

        _typeMapperMock.Setup(x => x.MapToDomain(fireTypeResponse))
            .Returns(fireType);
        _typeMapperMock.Setup(x => x.MapToDomain(waterTypeResponse))
            .Returns(waterType);

        // Act
        var result = await _service.GetAllTypesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(18); // All 18 Pokemon types
        
        // Verify cache was used
        _allTypesCacheMock.Verify(x => x.Get("all_types"), Times.Once);
        _allTypesCacheMock.Verify(x => x.Set("all_types", It.IsAny<List<PokemonType>>()), Times.Once);
    }

    [Fact]
    public async Task GetAllTypesAsync_WithCachedTypes_ShouldUseCache()
    {
        // Arrange
        var allTypes = new List<PokemonType>
        {
            new() { Id = 1, Name = "normal", Relations = new TypeRelations() },
            new() { Id = 2, Name = "fighting", Relations = new TypeRelations() }
        };

        // Setup cache to return cached data on second call
        _allTypesCacheMock.SetupSequence(x => x.Get("all_types"))
            .Returns((List<PokemonType>?)null)  // First call: cache miss
            .Returns(allTypes);                 // Second call: cache hit

        // Act - First call
        var result1 = await _service.GetAllTypesAsync();
        
        // Act - Second call (should use cache)
        var result2 = await _service.GetAllTypesAsync();

        // Assert
        result1.Should().HaveCount(18); // All 18 Pokemon types
        result2.Should().HaveCount(2);  // Cached types
        
        // Verify cache was checked twice
        _allTypesCacheMock.Verify(x => x.Get("all_types"), Times.Exactly(2));
        
        // Verify HTTP client was not called on second call (cache hit)
        _httpClientMock.Verify(x => x.GetAsync<TypeApiResponse>(It.IsAny<string>()), Times.Exactly(18));
    }

    [Fact]
    public async Task GetAllTypesAsync_WhenHttpClientThrowsException_ShouldRethrow()
    {
        // Arrange
        var expectedException = new HttpRequestException("API error");

        // Setup cache to return null (cache miss)
        _allTypesCacheMock.Setup(x => x.Get("all_types"))
            .Returns((List<PokemonType>?)null);

        _httpClientMock.Setup(x => x.GetAsync<TypeApiResponse>("type/normal"))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<HttpRequestException>(() => 
            _service.GetAllTypesAsync());
        
        exception.Should().Be(expectedException);
    }

    [Fact]
    public async Task GetAllTypesAsync_WhenIndividualTypeFails_ShouldRethrow()
    {
        // Arrange
        var expectedException = new HttpRequestException("Type API error");

        // Setup cache to return null (cache miss)
        _allTypesCacheMock.Setup(x => x.Get("all_types"))
            .Returns((List<PokemonType>?)null);

        _httpClientMock.Setup(x => x.GetAsync<TypeApiResponse>("type/normal"))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<HttpRequestException>(() => 
            _service.GetAllTypesAsync());
        
        exception.Should().Be(expectedException);
    }

    [Fact]
    public void TypeListResponse_WithValidData_ShouldBeCreatedCorrectly()
    {
        // Arrange & Act
        var response = new TypeListResponse
        {
            Count = 18,
            Next = "https://pokeapi.co/api/v2/type/?offset=20&limit=20",
            Previous = null,
            Results = new List<TypeReference>
            {
                new() { Name = "normal", Url = "https://pokeapi.co/api/v2/type/1/" },
                new() { Name = "fighting", Url = "https://pokeapi.co/api/v2/type/2/" }
            }
        };

        // Assert
        response.Count.Should().Be(18);
        response.Next.Should().Be("https://pokeapi.co/api/v2/type/?offset=20&limit=20");
        response.Previous.Should().BeNull();
        response.Results.Should().HaveCount(2);
        response.Results[0].Name.Should().Be("normal");
        response.Results[1].Name.Should().Be("fighting");
    }
}
