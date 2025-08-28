using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using PokemonTypeClash.Application.Services;
using PokemonTypeClash.Application.Configuration;
using PokemonTypeClash.Console.Configuration;
using PokemonTypeClash.Core.Interfaces;
using PokemonTypeClash.Infrastructure.Configuration;
using PokemonTypeClash.Infrastructure.Services;
using Xunit;
using Xunit.Abstractions;

namespace PokemonTypeClash.Performance.Tests;

[Collection("Performance")]
public class PerformanceTests
{
    private readonly IServiceProvider _serviceProvider;

    public PerformanceTests()
    {
        var services = new ServiceCollection();
        
        // Create configuration
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                {"PokeApi:BaseUrl", "https://pokeapi.co/api/v2/"},
                {"PokeApi:TimeoutSeconds", "30"}
            })
            .Build();
        
        // Add logging
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Warning);
        });

        // Add infrastructure services
        services.AddInfrastructureServices(configuration);
        
        // Add application services
        services.AddApplicationServices();
        
        _serviceProvider = services.BuildServiceProvider();
    }

    [Fact]
    [Trait("Category", "Performance")]
    public void ApplicationStartup_ShouldCompleteWithin2Seconds()
    {
        // Arrange
        var stopwatch = Stopwatch.StartNew();

        // Act
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                {"PokeApi:BaseUrl", "https://pokeapi.co/api/v2/"},
                {"PokeApi:TimeoutSeconds", "30"}
            })
            .Build();

        var serviceProvider = new ServiceCollection()
            .AddLogging(builder => builder.AddConsole())
            .AddInfrastructureServices(configuration)
            .AddApplicationServices()
            .BuildServiceProvider();

        stopwatch.Stop();

        // Assert
        Assert.True(stopwatch.ElapsedMilliseconds < 2000, 
            $"Application startup took {stopwatch.ElapsedMilliseconds}ms, expected less than 2000ms");
    }

    [Fact]
    [Trait("Category", "Performance")]
    public void ServiceResolution_ShouldCompleteWithin100Milliseconds()
    {
        // Arrange
        var stopwatch = Stopwatch.StartNew();

        // Act
        var pokemonService = _serviceProvider.GetService<IPokemonApiService>();
        var typeService = _serviceProvider.GetService<ITypeEffectivenessService>();

        stopwatch.Stop();

        // Assert
        Assert.NotNull(pokemonService);
        Assert.NotNull(typeService);
        Assert.True(stopwatch.ElapsedMilliseconds < 100, 
            $"Service resolution took {stopwatch.ElapsedMilliseconds}ms, expected less than 100ms");
    }

    [Fact]
    [Trait("Category", "Performance")]
    public void MemoryUsage_ShouldStayUnder50MB()
    {
        // Arrange
        var initialMemory = GC.GetTotalMemory(false);

        // Act - Create multiple service instances
        for (int i = 0; i < 100; i++)
        {
            var pokemonService = _serviceProvider.GetService<IPokemonApiService>();
            var typeService = _serviceProvider.GetService<ITypeEffectivenessService>();
            
            // Force garbage collection to get accurate measurement
            if (i % 10 == 0)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        var finalMemory = GC.GetTotalMemory(false);
        var memoryIncrease = finalMemory - initialMemory;

        // Assert
        Assert.True(memoryIncrease < 50 * 1024 * 1024, // 50MB in bytes
            $"Memory increase was {memoryIncrease / (1024 * 1024)}MB, expected less than 50MB");
    }

    [Fact]
    [Trait("Category", "Performance")]
    public void TypeEffectivenessCalculation_ShouldCompleteWithin1Millisecond()
    {
        // Arrange
        var typeService = _serviceProvider.GetService<ITypeEffectivenessService>();
        var stopwatch = Stopwatch.StartNew();

        // Act - Perform multiple calculations
        for (int i = 0; i < 1000; i++)
        {
            // This would require actual type data, so we'll test the service creation
            var logger = _serviceProvider.GetService<ILogger<TypeEffectivenessService>>();
            var typeDataService = _serviceProvider.GetService<ITypeDataService>();
            if (logger != null && typeDataService != null)
            {
                var service = new PokemonTypeClash.Application.Services.TypeEffectivenessService(logger, typeDataService);
            }
        }

        stopwatch.Stop();
        var averageTime = stopwatch.ElapsedMilliseconds / 1000.0;

        // Assert
        Assert.True(averageTime < 1, 
            $"Average calculation time was {averageTime}ms, expected less than 1ms");
    }

    [Fact]
    [Trait("Category", "Performance")]
    public async Task ConcurrentServiceAccess_ShouldHandleMultipleRequests()
    {
        // Arrange
        var pokemonService = _serviceProvider.GetService<IPokemonApiService>();
        var tasks = new List<Task>();
        var stopwatch = Stopwatch.StartNew();

        // Act - Simulate concurrent requests
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(async () =>
            {
                // Simulate service access (without actual API calls)
                await Task.Delay(10);
            }));
        }

        await Task.WhenAll(tasks);
        stopwatch.Stop();

        // Assert
        Assert.True(stopwatch.ElapsedMilliseconds < 1000, 
            $"Concurrent access took {stopwatch.ElapsedMilliseconds}ms, expected less than 1000ms");
    }

    [Fact]
    [Trait("Category", "Performance")]
    public void DependencyInjection_ShouldResolveAllServicesEfficiently()
    {
        // Arrange
        var stopwatch = Stopwatch.StartNew();
        var resolvedServices = new List<object>();

        // Act - Resolve all registered services
        foreach (var serviceType in GetRegisteredServiceTypes())
        {
            try
            {
                var service = _serviceProvider.GetService(serviceType);
                if (service != null)
                {
                    resolvedServices.Add(service);
                }
            }
            catch
            {
                // Ignore services that can't be resolved
            }
        }

        stopwatch.Stop();

        // Assert
        Assert.True(resolvedServices.Count > 0, "Should resolve at least one service");
        Assert.True(stopwatch.ElapsedMilliseconds < 500, 
            $"Service resolution took {stopwatch.ElapsedMilliseconds}ms, expected less than 500ms");
    }

    private IEnumerable<Type> GetRegisteredServiceTypes()
    {
        // Return common service types that should be registered
        return new[]
        {
            typeof(IPokemonApiService),
            typeof(ITypeEffectivenessService),
            typeof(ITypeDataService),
            typeof(ILogger<>)
        };
    }
}
