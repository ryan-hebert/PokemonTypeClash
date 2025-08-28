using Microsoft.Extensions.DependencyInjection;
using PokemonTypeClash.Application.Services;
using PokemonTypeClash.Core.Interfaces;

namespace PokemonTypeClash.Application.Configuration;

/// <summary>
/// Extension methods for configuring services in the Application layer
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Application services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register business logic services
        services.AddScoped<ITypeEffectivenessService, TypeEffectivenessService>();
        
        return services;
    }
}
