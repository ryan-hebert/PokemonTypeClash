using System.Diagnostics.CodeAnalysis;
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
    [RequiresUnreferencedCode("Configuration binding and dependency injection might require types that cannot be statically analyzed.")]
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

            // Add configuration
            services.AddSingleton<IConfiguration>(configuration);

            // Add logging - disable console logging for end users to keep output clean
            services.AddLogging(builder =>
            {
                // Only enable logging in debug mode
                #if DEBUG
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Warning);
                #endif
            });

            // Add Console services (includes Infrastructure and Application services)
            services.AddConsoleServices(configuration);

            // Build service provider
            var serviceProvider = services.BuildServiceProvider();



            // Get the console UI service
            var consoleUI = serviceProvider.GetRequiredService<IConsoleUI>();

            // Check if command-line arguments are provided
            if (args.Length > 0)
            {
                // Run in command-line mode (for CI/testing)
                return await RunCommandLineMode(consoleUI, args);
            }
            else
            {
                // Run in interactive mode (for users)
                await consoleUI.RunAsync();
                return 0;
            }
        }
        catch (Exception
            #if DEBUG
            ex
            #endif
        )
        {
            // Graceful error handling - don't show technical details to end users
            System.Console.WriteLine();
            System.Console.WriteLine("❌ An unexpected error occurred. Please try again.");
            System.Console.WriteLine();
            System.Console.WriteLine("If the problem persists, please check:");
            System.Console.WriteLine("• Your internet connection");
            System.Console.WriteLine("• That you're using the latest version");
            System.Console.WriteLine();
            
            // Log the actual error for debugging (only in development)
            #if DEBUG
            System.Console.WriteLine($"Debug Info: {ex.Message}");
            #endif
            
            return 1;
        }
    }

    /// <summary>
    /// Runs the application in command-line mode for CI/testing
    /// </summary>
    /// <param name="consoleUI">The console UI service</param>
    /// <param name="args">Command line arguments</param>
    /// <returns>Exit code</returns>
    private static async Task<int> RunCommandLineMode(IConsoleUI consoleUI, string[] args)
    {
        try
        {
            // Set command-line mode to avoid interactive prompts
            consoleUI.IsCommandLineMode = true;
            
            // Parse command and arguments
            var command = args[0].ToLowerInvariant();
            var commandArgs = args.Skip(1).ToArray();

            switch (command)
            {
                case "--analyze":
                case "-a":
                    if (commandArgs.Length == 0)
                    {
                        System.Console.WriteLine("Error: Pokemon name or ID required for analysis.");
                        System.Console.WriteLine("Usage: --analyze <pokemon-name-or-id>");
                        return 1;
                    }
                    
                    var pokemonName = commandArgs[0];
                    await consoleUI.RunAnalysisAsync(pokemonName);
                    return 0;

                case "--help":
                case "-h":
                case "-?":
                    consoleUI.ShowHelp();
                    return 0;

                case "--version":
                case "-v":
                    System.Console.WriteLine("PokemonTypeClash v1.0.0");
                    return 0;

                default:
                    System.Console.WriteLine($"Error: Unknown command '{command}'");
                    System.Console.WriteLine("Available commands:");
                    System.Console.WriteLine("  --analyze <pokemon>  Analyze a Pokemon's type effectiveness");
                    System.Console.WriteLine("  --help              Show help information");
                    System.Console.WriteLine("  --version           Show version information");
                    return 1;
            }
        }
        catch (Exception
            #if DEBUG
            ex
            #endif
        )
        {
            // Graceful error handling for command-line mode
            System.Console.WriteLine("❌ An error occurred while processing your request.");
            System.Console.WriteLine("Please check your input and try again.");
            
            // Log the actual error for debugging (only in development)
            #if DEBUG
            System.Console.WriteLine($"Debug Info: {ex.Message}");
            #endif
            
            return 1;
        }
    }
}
