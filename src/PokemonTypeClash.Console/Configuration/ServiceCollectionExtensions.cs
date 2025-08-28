using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PokemonTypeClash.Application.Configuration;
using PokemonTypeClash.Core.Interfaces;
using PokemonTypeClash.Infrastructure.Configuration;
using PokemonTypeClash.Console.UI;

namespace PokemonTypeClash.Console.Configuration;

/// <summary>
/// Extension methods for configuring services in the Console layer
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Console services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <returns>The service collection</returns>
    public static IServiceCollection AddConsoleServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Infrastructure services
        services.AddInfrastructureServices(configuration);
        
        // Add Application services
        services.AddApplicationServices();
        
        // Add Console services
        services.AddScoped<IConsoleUI, ConsoleUI>();
        
        return services;
    }
}
