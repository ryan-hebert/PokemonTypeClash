using PokemonTypeClash.Core.Interfaces;
using PokemonTypeClash.Core.Models;

namespace PokemonTypeClash.Core.Interfaces;

/// <summary>
/// Service for handling console user interface operations
/// </summary>
public interface IConsoleUI
{
    /// <summary>
    /// Displays the main menu to the user
    /// </summary>
    void ShowMainMenu();
    
    /// <summary>
    /// Displays the main menu to the user with arrow key navigation
    /// </summary>
    /// <returns>The selected menu option (1-4)</returns>
    string ShowMainMenuWithNavigation();
    
    /// <summary>
    /// Gets user input with a prompt
    /// </summary>
    /// <param name="prompt">The prompt to display to the user</param>
    /// <returns>The user's input</returns>
    string GetUserInput(string prompt);
    
    /// <summary>
    /// Displays the type analysis result to the user
    /// </summary>
    /// <param name="result">The analysis result to display</param>
    void DisplayAnalysisResult(TypeAnalysisResult result);
    
    /// <summary>
    /// Displays an error message to the user
    /// </summary>
    /// <param name="message">The error message to display</param>
    void ShowError(string message);
    
    /// <summary>
    /// Displays a success message to the user
    /// </summary>
    /// <param name="message">The success message to display</param>
    void ShowSuccess(string message);
    
    /// <summary>
    /// Displays a loading message to the user
    /// </summary>
    /// <param name="message">The loading message to display</param>
    void ShowLoading(string message);
    
    /// <summary>
    /// Clears the loading message
    /// </summary>
    void ClearLoading();
    
    /// <summary>
    /// Displays help information and examples
    /// </summary>
    void ShowHelp();
    
    /// <summary>
    /// Prompts the user to continue or exit
    /// </summary>
    /// <returns>True if the user wants to continue, false to exit</returns>
    bool AskToContinue();
    
    /// <summary>
    /// Runs the main application loop
    /// </summary>
    Task RunAsync();

    /// <summary>
    /// Runs a Pokemon analysis without interactive prompts (for CI/testing)
    /// </summary>
    /// <param name="pokemonName">The name or ID of the Pokemon to analyze</param>
    Task RunAnalysisAsync(string pokemonName);

    /// <summary>
    /// Gets or sets whether the application is running in command-line mode
    /// </summary>
    bool IsCommandLineMode { get; set; }
}
