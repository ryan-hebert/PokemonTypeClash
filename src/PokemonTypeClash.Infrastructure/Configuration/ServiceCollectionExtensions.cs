using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PokemonTypeClash.Infrastructure.Mappers;
using PokemonTypeClash.Core.Interfaces;
using PokemonTypeClash.Infrastructure.Http;
using PokemonTypeClash.Infrastructure.Services;
using PokemonTypeClash.Core.Models;

namespace PokemonTypeClash.Infrastructure.Configuration;

/// <summary>
/// Extension methods for configuring services in the Infrastructure layer
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Infrastructure services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <returns>The service collection</returns>
    [RequiresUnreferencedCode("Configuration binding and dependency injection might require types that cannot be statically analyzed.")]
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure PokeAPI settings
        services.Configure<PokeApiConfiguration>(configuration.GetSection("PokeApi"));
        
        // Register PokeApiConfiguration as singleton for direct injection
        var pokeApiConfig = new PokeApiConfiguration();
        configuration.GetSection("PokeApi").Bind(pokeApiConfig);
        services.AddSingleton(pokeApiConfig);
        
        // Register cache services
        services.AddSingleton<ICacheService<Pokemon>, CacheService<Pokemon>>();
        services.AddSingleton<ICacheService<PokemonType>, CacheService<PokemonType>>();
        services.AddSingleton<ICacheService<List<PokemonType>>, CacheService<List<PokemonType>>>();
        
        // Register mappers
        services.AddScoped<IPokemonMapper, PokemonMapper>();
        services.AddScoped<ITypeMapper, TypeMapper>();
        
        // Register services
        services.AddScoped<IPokemonApiService, PokemonApiService>();
        services.AddScoped<ITypeDataService, TypeDataService>();
        
        // Configure HttpClient with standard settings (no SSL bypass needed with VPN)
        services.AddHttpClient<PokeApiHttpClient>(client =>
        {
            client.BaseAddress = new Uri(pokeApiConfig.BaseUrl);
            client.DefaultRequestHeaders.Add("User-Agent", "PokemonTypeClash/1.0");
            client.Timeout = TimeSpan.FromSeconds(pokeApiConfig.TimeoutSeconds);
        });
        
        // Register the interface to point to the concrete implementation
        services.AddScoped<IPokeApiHttpClient>(provider => provider.GetRequiredService<PokeApiHttpClient>());
        
        return services;
    }
}
