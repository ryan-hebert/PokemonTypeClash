using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PokemonTypeClash.Console.Configuration;
using PokemonTypeClash.Core.Interfaces;
using PokemonTypeClash.Infrastructure.Http;

namespace PokemonTypeClash.Console;

/// <summary>
/// Main entry point for the PokemonTypeClash application
/// </summary>
public class Program
{
    /// <summary>
    /// Main entry point
    /// </summary>
    /// <param name="args">Command line arguments</param>
    /// <returns>Exit code</returns>
    public static async Task<int> Main(string[] args)
    {
        try
        {
            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            // Build service collection
            var services = new ServiceCollection();

            // Add logging - only show warnings and errors to keep user output clean
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Warning);
            });

            // Add Console services (includes Infrastructure and Application services)
            services.AddConsoleServices(configuration);

            // Build service provider
            var serviceProvider = services.BuildServiceProvider();

            // Get the console UI service
            var consoleUI = serviceProvider.GetRequiredService<IConsoleUI>();

            // Run the application
            await consoleUI.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Fatal error: {ex.Message}");
            return 1;
        }
    }
}
