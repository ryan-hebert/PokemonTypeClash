using System.Diagnostics.CodeAnalysis;
using PokemonTypeClash.Core.Interfaces;
using PokemonTypeClash.Core.Models;
using PokemonTypeClash.Core.Exceptions;
using Spectre.Console;

namespace PokemonTypeClash.Console.UI;

/// <summary>
/// Spectre.Console-based implementation of the console user interface service
/// Provides consistent cross-platform UI rendering
/// </summary>
public class SpectreConsoleUI : IConsoleUI
{
    private readonly ITypeEffectivenessService _typeEffectivenessService;
    private readonly IPokemonApiService _pokemonApiService;

    public SpectreConsoleUI(ITypeEffectivenessService typeEffectivenessService, IPokemonApiService pokemonApiService)
    {
        _typeEffectivenessService = typeEffectivenessService;
        _pokemonApiService = pokemonApiService;
    }

    /// <summary>
    /// Gets or sets whether the application is running in command-line mode
    /// </summary>
    public bool IsCommandLineMode { get; set; }

    /// <summary>
    /// Displays the main menu to the user
    /// </summary>
    public void ShowMainMenu()
    {
        AnsiConsole.Clear();
        
        // Pokemon-themed header with consistent rendering
        var header = new FigletText("PokemonTypeClash")
            .Centered()
            .Color(Color.Yellow);
        
        AnsiConsole.Write(header);
        AnsiConsole.WriteLine();
        
        // Menu options with consistent styling
        AnsiConsole.MarkupLine("[yellow]⚡ 1.[/] [white]Analyze Pokemon Type Effectiveness[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[yellow]📊 2.[/] [white]View All Pokemon Types[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[yellow]❓ 3.[/] [white]Help & Examples[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[yellow]🚪 4.[/] [white]Exit[/]");
        AnsiConsole.WriteLine();
        
        var rule = new Rule("[blue]PokemonTypeClash[/]")
            .Centered();
        AnsiConsole.Write(rule);
        AnsiConsole.WriteLine();
    }

    /// <summary>
    /// Displays the main menu to the user with arrow key navigation
    /// </summary>
    /// <returns>The selected menu option (1-4)</returns>
    public string ShowMainMenuWithNavigation()
    {
        AnsiConsole.Clear();
        
        // Pokemon-themed header with consistent rendering
        var header = new FigletText("PokemonTypeClash")
            .Centered()
            .Color(Color.Yellow);
        
        AnsiConsole.Write(header);
        AnsiConsole.WriteLine();
        
        // Navigation instructions
        var navigationPanel = new Panel("[cyan]💡 Use ↑↓ arrow keys to navigate and Enter to select[/]")
            .BorderColor(Color.Blue)
            .Padding(new Padding(0, 0));
        
        AnsiConsole.Write(navigationPanel);
        AnsiConsole.WriteLine();
        
        // Menu options with consistent styling
        var choices = new[]
        {
            "⚡ Analyze Pokemon Type Effectiveness",
            "📊 View All Pokemon Types", 
            "❓ Help & Examples",
            "🚪 Exit"
        };
        
        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[blue]Choose an option:[/]")
                .PageSize(10)
                .AddChoices(choices)
        );
        
        // Return the selected option number (1-based index)
        var selectedIndex = Array.IndexOf(choices, selection);
        return (selectedIndex + 1).ToString();
    }

    /// <summary>
    /// Gets user input with a prompt
    /// </summary>
    /// <param name="prompt">The prompt to display to the user</param>
    /// <returns>The user's input</returns>
    public string GetUserInput(string prompt)
    {
        return AnsiConsole.Ask<string>($"[yellow]🎯 {prompt}:[/]");
    }

    /// <summary>
    /// Validates and sanitizes Pokemon name input
    /// </summary>
    /// <param name="input">The raw user input</param>
    /// <returns>Tuple of (isValid, sanitizedInput, errorMessage)</returns>
    private (bool isValid, string sanitizedInput, string? errorMessage) ValidatePokemonInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return (false, "", "Pokemon name cannot be empty");
        }

        // Trim whitespace
        var trimmed = input.Trim();
        
        // Check if it's a valid number (Pokemon ID)
        if (int.TryParse(trimmed, out int pokemonId))
        {
            if (pokemonId <= 0 || pokemonId > 1025) // Reasonable range for Pokemon IDs
            {
                return (false, "", "Pokemon ID must be between 1 and 1025");
            }
            return (true, trimmed, null);
        }

        // For text input, validate characters
        var sanitized = trimmed.ToLowerInvariant();
        
        // Only allow letters, numbers, and hyphens (for Pokemon like "mr-mime")
        if (!sanitized.All(c => char.IsLetterOrDigit(c) || c == '-'))
        {
            return (false, "", "Pokemon name can only contain letters, numbers, and hyphens");
        }

        // Check length
        if (sanitized.Length < 1 || sanitized.Length > 20)
        {
            return (false, "", "Pokemon name must be between 1 and 20 characters");
        }

        // Check for consecutive hyphens or hyphens at start/end
        if (sanitized.StartsWith("-") || sanitized.EndsWith("-") || sanitized.Contains("--"))
        {
            return (false, "", "Pokemon name cannot start/end with hyphens or have consecutive hyphens");
        }

        return (true, sanitized, null);
    }

    /// <summary>
    /// Displays the type analysis result to the user with enhanced rich tables
    /// </summary>
    /// <param name="result">The analysis result to display</param>
    public async Task DisplayAnalysisResult(TypeAnalysisResult result)
    {
        AnsiConsole.Clear();
        
        // Header with consistent styling
        var header = new Rule("[yellow]TYPE ANALYSIS[/]")
            .Centered();
        AnsiConsole.Write(header);
        AnsiConsole.WriteLine();
        
        // Pokemon info in a panel with enhanced styling
        var pokemonPanel = new Panel($"[yellow]🐶 {result.Pokemon.Name.ToUpperInvariant()}[/]\n\n[white]⚡ Types: {string.Join(", ", result.Pokemon.Types.Select(t => t.Name))}[/]")
            .Header("[blue]Pokemon Info[/]")
            .BorderColor(Color.Yellow)
            .Padding(new Padding(1, 0));
        
        AnsiConsole.Write(pokemonPanel);
        AnsiConsole.WriteLine();
        
        // Enhanced offensive analysis table
        var offensiveTable = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Green)
            .Title("[green]⚔️ OFFENSIVE CAPABILITIES[/]")
            .Width(80);
        
        offensiveTable.AddColumn(new TableColumn("[green]Strong Against[/]").LeftAligned());
        offensiveTable.AddColumn(new TableColumn("[red]Weak Against[/]").LeftAligned());
        offensiveTable.AddColumn(new TableColumn("[gray]No Effect[/]").LeftAligned());
        
        // Add offensive data with proper formatting
        var strongAgainst = result.StrongAgainst.Any() 
            ? string.Join("\n", result.StrongAgainst.Select(t => $"[green]• {t.Name}[/]"))
            : "[gray]None[/]";
            
        var weakAgainst = result.WeakAgainst.Any() 
            ? string.Join("\n", result.WeakAgainst.Select(t => $"[red]• {t.Name}[/]"))
            : "[gray]None[/]";
            
        var noEffect = result.NoEffectAgainst.Any() 
            ? string.Join("\n", result.NoEffectAgainst.Select(t => $"[gray]• {t.Name}[/]"))
            : "[gray]None[/]";
        
        offensiveTable.AddRow(strongAgainst, weakAgainst, noEffect);
        
        AnsiConsole.Write(offensiveTable);
        AnsiConsole.WriteLine();
        
        // Enhanced defensive analysis table
        var defensiveTable = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Blue)
            .Title("[blue]🛡️ DEFENSIVE CAPABILITIES[/]")
            .Width(80);
        
        defensiveTable.AddColumn(new TableColumn("[blue]Resistant To[/]").LeftAligned());
        defensiveTable.AddColumn(new TableColumn("[yellow]Vulnerable To[/]").LeftAligned());
        defensiveTable.AddColumn(new TableColumn("[purple]Immune To[/]").LeftAligned());
        
        // Add defensive data with proper formatting
        var resistantTo = result.ResistantTo.Any() 
            ? string.Join("\n", result.ResistantTo.Select(t => $"[blue]• {t.Name}[/]"))
            : "[gray]None[/]";
            
        var vulnerableTo = result.VulnerableTo.Any() 
            ? string.Join("\n", result.VulnerableTo.Select(t => $"[yellow]• {t.Name}[/]"))
            : "[gray]None[/]";
            
        var immuneTo = result.ImmuneTo.Any() 
            ? string.Join("\n", result.ImmuneTo.Select(t => $"[purple]• {t.Name}[/]"))
            : "[gray]None[/]";
        
        defensiveTable.AddRow(resistantTo, vulnerableTo, immuneTo);
        
        AnsiConsole.Write(defensiveTable);
        AnsiConsole.WriteLine();
        
        // Battle summary panel
        var summaryContent = $"[white]🎯 Battle Summary for {result.Pokemon.Name.ToUpperInvariant()}:[/]\n\n";
        summaryContent += $"[green]Strong against: {result.StrongAgainst.Count} types[/]\n";
        summaryContent += $"[red]Weak against: {result.WeakAgainst.Count} types[/]\n";
        summaryContent += $"[blue]Resistant to: {result.ResistantTo.Count} types[/]\n";
        summaryContent += $"[yellow]Vulnerable to: {result.VulnerableTo.Count} types[/]\n";
        
        if (result.ImmuneTo.Any())
        {
            summaryContent += $"[purple]Immune to: {result.ImmuneTo.Count} types[/]\n";
        }
        
        var summaryPanel = new Panel(summaryContent)
            .Header("[cyan]📊 Battle Statistics[/]")
            .BorderColor(Color.Blue)
            .Padding(new Padding(1, 0));
        
        AnsiConsole.Write(summaryPanel);
        
        // Wait for user input if not in command-line mode
        if (!IsCommandLineMode)
        {
            AnsiConsole.WriteLine();
            
            // Navigation instructions for next action
            AnsiConsole.MarkupLine("[cyan]💡 Use ↑↓ arrow keys to navigate and Enter to select[/]");
            AnsiConsole.WriteLine();
            
            // Ask if user wants to analyze another Pokemon
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]What would you like to do next?[/]")
                    .AddChoices("🎯 Analyze Another Pokemon", "🏠 Return to Main Menu")
            );
            
            if (choice == "🎯 Analyze Another Pokemon")
            {
                // Clear screen and analyze another Pokemon
                AnsiConsole.Clear();
                await AnalyzePokemonAsync();
            }
            // If they choose to return to main menu, the method will return and the main loop will continue
        }
    }

    /// <summary>
    /// Shows an error message with enhanced styling
    /// </summary>
    /// <param name="message">The error message to display</param>
    public void ShowError(string message)
    {
        var errorPanel = new Panel($"[red]{message}[/]")
            .Header("[red]❌ Error[/]")
            .BorderColor(Color.Red)
            .Padding(new Padding(1, 0));
        
        AnsiConsole.Write(errorPanel);
        AnsiConsole.WriteLine();
    }

    /// <summary>
    /// Displays a success message to the user with enhanced styling
    /// </summary>
    /// <param name="message">The success message to display</param>
    public void ShowSuccess(string message)
    {
        var successPanel = new Panel($"[green]{message}[/]")
            .Header("[green]✅ Success[/]")
            .BorderColor(Color.Green)
            .Padding(new Padding(1, 0));
        
        AnsiConsole.Write(successPanel);
        AnsiConsole.WriteLine();
    }

    /// <summary>
    /// Shows an enhanced error message with recovery suggestions
    /// </summary>
    /// <param name="message">The error message to display</param>
    /// <param name="suggestions">Optional recovery suggestions</param>
    public void ShowEnhancedError(string message, string[]? suggestions = null)
    {
        var errorContent = $"[red]{message}[/]";
        
        if (suggestions != null && suggestions.Length > 0)
        {
            errorContent += "\n\n[white]💡 Suggestions:[/]\n";
            foreach (var suggestion in suggestions)
            {
                errorContent += $"[cyan]• {suggestion}[/]\n";
            }
        }
        
        var errorPanel = new Panel(errorContent)
            .Header("[red]❌ Error[/]")
            .BorderColor(Color.Red)
            .Padding(new Padding(1, 0));
        
        AnsiConsole.Write(errorPanel);
        AnsiConsole.WriteLine();
    }

    /// <summary>
    /// Shows a warning message with enhanced styling
    /// </summary>
    /// <param name="message">The warning message to display</param>
    public void ShowWarning(string message)
    {
        var warningPanel = new Panel($"[yellow]{message}[/]")
            .Header("[yellow]⚠️ Warning[/]")
            .BorderColor(Color.Yellow)
            .Padding(new Padding(1, 0));
        
        AnsiConsole.Write(warningPanel);
        AnsiConsole.WriteLine();
    }

    /// <summary>
    /// Demonstrates all enhanced message types for testing
    /// </summary>
    public void ShowMessageDemo()
    {
        AnsiConsole.Clear();
        
        var header = new Rule("[yellow]ENHANCED MESSAGE DEMO[/]")
            .Centered();
        AnsiConsole.Write(header);
        AnsiConsole.WriteLine();
        
        // Show success message
        ShowSuccess("Pokemon analysis completed successfully!");
        
        // Show warning message
        ShowWarning("This is a sample warning message for demonstration.");
        
        // Show enhanced error with suggestions
        ShowEnhancedError("Failed to connect to Pokemon API", new[]
        {
            "Check your internet connection",
            "Verify the API endpoint is accessible",
            "Try again in a few moments"
        });
        
        // Show simple error
        ShowError("This is a simple error message.");
        
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[cyan]Press any key to continue...[/]");
        AnsiConsole.Console.Input.ReadKey(true);
    }

    /// <summary>
    /// Displays a loading message to the user
    /// </summary>
    /// <param name="message">The loading message to display</param>
    public void ShowLoading(string message)
    {
        // Simple loading message without complex animations
        AnsiConsole.WriteLine($"⏳ {message}...");
        Thread.Sleep(500); // Brief pause to show loading
    }

    /// <summary>
    /// Clears the loading message
    /// </summary>
    public void ClearLoading()
    {
        // Spectre.Console handles this automatically
        // No explicit clearing needed
    }

    /// <summary>
    /// Displays enhanced help information and examples
    /// </summary>
    public void ShowHelp()
    {
        AnsiConsole.Clear();
        
        var header = new Rule("[yellow]HELP & EXAMPLES[/]")
            .Centered();
        AnsiConsole.Write(header);
        AnsiConsole.WriteLine();
        
        // Getting Started Panel
        var gettingStartedPanel = new Panel(@"
[white]🎯 How to use PokemonTypeClash:[/]

[green]1. Choose 'Analyze Pokemon Type Effectiveness'[/]
[green]2. Select from popular Pokemon or enter manually[/]
[green]3. View the detailed battle analysis[/]
[green]4. Choose to analyze another Pokemon or return to menu[/]

[white]💡 Pro Tips:[/]
• Pokemon names are case-insensitive
• You can use Pokemon numbers (e.g., '25' for Pikachu)
• Each Pokemon has unique type combinations
• Study the effectiveness to become a master!
• Use the 'View All Pokemon Types' to learn type relationships
")
            .Header("[blue]🚀 Getting Started[/]")
            .BorderColor(Color.Blue)
            .Padding(new Padding(1, 0));
        
        AnsiConsole.Write(gettingStartedPanel);
        AnsiConsole.WriteLine();
        
        // Example Pokemon Panel
        var examplePanel = new Panel(@"
[white]🎮 Popular Pokemon to try:[/]

[red]• Pikachu (#25) - Electric[/]
[blue]• Charizard (#6) - Fire/Flying[/]
[green]• Venusaur (#3) - Grass/Poison[/]
[cyan]• Gyarados (#130) - Water/Flying[/]
[magenta]• Lucario (#448) - Fighting/Steel[/]
[yellow]• Mewtwo (#150) - Psychic[/]
[gray]• Garchomp (#445) - Dragon/Ground[/]
[purple]• Arceus (#493) - Normal[/]
")
            .Header("[green]🌟 Example Pokemon[/]")
            .BorderColor(Color.Green)
            .Padding(new Padding(1, 0));
        
        AnsiConsole.Write(examplePanel);
        AnsiConsole.WriteLine();
        
        // Type Effectiveness Guide Panel
        var effectivenessPanel = new Panel(@"
[white]⚔️ Understanding Type Effectiveness:[/]

[green]Super Effective (2x damage):[/] Strong against these types
[red]Not Very Effective (0.5x damage):[/] Weak against these types
[blue]Resistant (0.5x damage taken):[/] Takes reduced damage from these types
[yellow]Vulnerable (2x damage taken):[/] Takes increased damage from these types
[purple]Immune (0x damage taken):[/] Takes no damage from these types

[white]💡 Battle Strategy:[/]
• Use type advantages for maximum damage
• Consider dual-type Pokemon for complex matchups
• Some types have no weaknesses (like Shedinja with Wonder Guard)
• Weather and abilities can modify type effectiveness
")
            .Header("[yellow]📚 Type Effectiveness Guide[/]")
            .BorderColor(Color.Yellow)
            .Padding(new Padding(1, 0));
        
        AnsiConsole.Write(effectivenessPanel);
        
        // Wait for user input if not in command-line mode
        if (!IsCommandLineMode)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[cyan]Press any key to continue...[/]");
            AnsiConsole.Console.Input.ReadKey(true);
        }
    }

    /// <summary>
    /// Prompts the user to continue or exit
    /// </summary>
    /// <returns>True if the user wants to continue, false to exit</returns>
    public bool AskToContinue()
    {
        if (IsCommandLineMode)
        {
            return true; // Auto-continue in command-line mode
        }
        
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[blue]Would you like to continue?[/]")
                .AddChoices("Yes", "No")
        );
        
        return choice == "Yes";
    }

    /// <summary>
    /// Clears the console screen
    /// </summary>
    public void ClearScreen()
    {
        AnsiConsole.Clear();
    }

    /// <summary>
    /// Shows all Pokemon types with enhanced rich tables and graceful error handling
    /// </summary>
    public async Task ShowAllTypesAsync()
    {
        try
        {
            AnsiConsole.Clear();
            
            var header = new Rule("[yellow]ALL POKEMON TYPES[/]")
                .Centered();
            AnsiConsole.Write(header);
            AnsiConsole.WriteLine();
            
            // Show loading while preparing the type chart
            ShowLoading("📊 Preparing Pokemon type chart...");
            await Task.Delay(1000); // Simulate loading time
        
        // Create a comprehensive type overview table
        var typeOverviewTable = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Green)
            .Title("[green]🎨 Pokemon Type Overview[/]")
            .Width(80);
        
        typeOverviewTable.AddColumn(new TableColumn("[white]Type[/]").LeftAligned().Width(15));
        typeOverviewTable.AddColumn(new TableColumn("[white]Description[/]").LeftAligned().Width(65));
        
        // Add all Pokemon types with descriptions
        var typeData = new[]
        {
            ("[red]Fire[/]", "Burns with intense heat and passion"),
            ("[blue]Water[/]", "Flows with adaptability and strength"),
            ("[green]Grass[/]", "Grows with nature's power and vitality"),
            ("[yellow]Electric[/]", "Crackles with lightning speed and energy"),
            ("[gray]Normal[/]", "Balanced and versatile in battle"),
            ("[red]Fighting[/]", "Strikes with martial arts and discipline"),
            ("[cyan]Flying[/]", "Soars with freedom and aerial advantage"),
            ("[magenta]Poison[/]", "Corrodes with toxic substances"),
            ("[yellow]Ground[/]", "Stands firm with earth's stability"),
            ("[gray]Rock[/]", "Endures with unbreakable solidity"),
            ("[green]Bug[/]", "Swarms with insect precision"),
            ("[magenta]Ghost[/]", "Haunts with supernatural abilities"),
            ("[gray]Steel[/]", "Shines with metallic durability"),
            ("[red]Psychic[/]", "Minds with psychic powers"),
            ("[cyan]Ice[/]", "Freezes with crystalline beauty"),
            ("[magenta]Dragon[/]", "Roars with legendary might"),
            ("[gray]Dark[/]", "Shadows with mysterious power"),
            ("[magenta]Fairy[/]", "Enchants with magical charm")
        };
        
        foreach (var (type, description) in typeData)
        {
            typeOverviewTable.AddRow(new[] { type, description });
        }
        
        AnsiConsole.Write(typeOverviewTable);
        AnsiConsole.WriteLine();
        
        // Create a type effectiveness summary table
        var effectivenessTable = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Blue)
            .Title("[blue]⚔️ Type Effectiveness Examples[/]")
            .Width(80);
        
        effectivenessTable.AddColumn(new TableColumn("[white]Type[/]").LeftAligned());
        effectivenessTable.AddColumn(new TableColumn("[green]Strong Against[/]").LeftAligned());
        effectivenessTable.AddColumn(new TableColumn("[red]Weak Against[/]").LeftAligned());
        effectivenessTable.AddColumn(new TableColumn("[blue]Resistant To[/]").LeftAligned());
        effectivenessTable.AddColumn(new TableColumn("[yellow]Vulnerable To[/]").LeftAligned());
        
        // Add comprehensive type effectiveness data
        var effectivenessData = new[]
        {
            ("[yellow]Electric[/]", "[green]Water, Flying[/]", "[red]Ground, Grass, Dragon[/]", "[blue]Electric, Flying, Steel[/]", "[yellow]Ground[/]"),
            ("[red]Fire[/]", "[green]Grass, Ice, Bug, Steel[/]", "[red]Water, Rock, Dragon[/]", "[blue]Fire, Grass, Ice, Bug, Steel[/]", "[yellow]Water, Ground, Rock[/]"),
            ("[blue]Water[/]", "[green]Fire, Ground, Rock[/]", "[red]Water, Grass, Dragon[/]", "[blue]Fire, Water, Ice, Steel[/]", "[yellow]Electric, Grass[/]"),
            ("[green]Grass[/]", "[green]Water, Ground, Rock[/]", "[red]Fire, Grass, Poison, Flying, Bug, Dragon, Steel[/]", "[blue]Water, Electric, Grass, Ground[/]", "[yellow]Fire, Ice, Poison, Flying, Bug[/]"),
            ("[gray]Normal[/]", "[green]None[/]", "[red]Rock, Steel[/]", "[blue]None[/]", "[yellow]Fighting[/]"),
            ("[red]Fighting[/]", "[green]Normal, Ice, Rock, Steel, Dark[/]", "[red]Poison, Flying, Psychic, Bug, Fairy[/]", "[blue]Rock, Bug, Dark[/]", "[yellow]Flying, Psychic, Fairy[/]"),
            ("[cyan]Flying[/]", "[green]Grass, Fighting, Bug[/]", "[red]Electric, Rock, Steel[/]", "[blue]Grass, Fighting, Bug[/]", "[yellow]Electric, Ice, Rock[/]"),
            ("[magenta]Poison[/]", "[green]Grass, Fairy[/]", "[red]Poison, Ground, Rock, Ghost, Steel[/]", "[blue]Grass, Fighting, Poison, Bug, Fairy[/]", "[yellow]Ground, Psychic[/]")
        };
        
        foreach (var row in effectivenessData)
        {
            effectivenessTable.AddRow(new[] { row.Item1, row.Item2, row.Item3, row.Item4, row.Item5 });
        }
        
        AnsiConsole.Write(effectivenessTable);
        AnsiConsole.WriteLine();
        
        // Enhanced legend with more details
        var legendPanel = new Panel(@"
[white]🎯 Type Effectiveness Guide:[/]

[green]Strong Against[/] - Super effective attacks (2x damage)
[red]Weak Against[/] - Not very effective attacks (0.5x damage)
[blue]Resistant To[/] - Takes reduced damage (0.5x damage)
[yellow]Vulnerable To[/] - Takes super effective damage (2x damage)
[purple]Immune To[/] - Takes no damage (0x damage)

[white]💡 Battle Tips:[/]
• Use type advantages for maximum damage
• Consider dual-type Pokemon for complex matchups
• Some types have no weaknesses (like Shedinja with Wonder Guard)
• Weather and abilities can modify type effectiveness
")
            .Header("[blue]📚 Type Effectiveness Guide[/]")
            .BorderColor(Color.Yellow)
            .Padding(new Padding(1, 0));
        
        AnsiConsole.Write(legendPanel);
        
        // Wait for user input if not in command-line mode
        if (!IsCommandLineMode)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[cyan]Press any key to continue...[/]");
            AnsiConsole.Console.Input.ReadKey(true);
        }
        }
        catch (Exception)
        {
            // Handle any errors gracefully
            ShowEnhancedError("Failed to display Pokemon types", new[]
            {
                "Please try again",
                "If the problem persists, restart the application"
            });
        }
    }

    /// <summary>
    /// Runs the main application loop with graceful error handling
    /// </summary>
    [RequiresUnreferencedCode("Spectre.Console uses reflection for rendering")]
    public async Task RunAsync()
    {
        try
        {
            while (true)
            {
                try
                {
                    var choice = ShowMainMenuWithNavigation();
                    
                    switch (choice)
                    {
                        case "1":
                            await AnalyzePokemonAsync();
                            break;
                        case "2":
                            await ShowAllTypesAsync();
                            break;
                        case "3":
                            ShowHelp();
                            break;
                        case "4":
                            AnsiConsole.MarkupLine("[yellow]Thanks for playing PokemonTypeClash! ⚡[/]");
                            return;
                        default:
                            ShowEnhancedError("Invalid option selected", new[] 
                            {
                                "Please select a valid option from the menu (1-4)",
                                "Use arrow keys to navigate and Enter to select"
                            });
                            break;
                    }
                }
                catch (Exception)
                {
                    // Handle any unexpected errors gracefully
                    ShowEnhancedError("Something went wrong", new[]
                    {
                        "Please try again",
                        "If the problem persists, restart the application"
                    });
                    
                    // Wait a moment before continuing
                    await Task.Delay(2000);
                }
            }
        }
        catch (Exception)
        {
            // Final fallback for critical errors
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[red]❌ An unexpected error occurred.[/]");
            AnsiConsole.MarkupLine("[yellow]The application will now exit.[/]");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[cyan]Press any key to exit...[/]");
            AnsiConsole.Console.Input.ReadKey(true);
        }
    }

    /// <summary>
    /// Analyzes a Pokemon's type effectiveness with graceful error handling
    /// </summary>
    /// <param name="pokemonName">The name of the Pokemon to analyze</param>
    [RequiresUnreferencedCode("Spectre.Console uses reflection for rendering")]
    public async Task RunAnalysisAsync(string pokemonName)
    {
        try
        {
            // Step 1: Fetch Pokemon data
            ShowLoading($"🔍 Searching for {pokemonName}...");
            var pokemon = await _pokemonApiService.GetPokemonAsync(pokemonName);
            
            // Show success message for Pokemon found
            ShowSuccess($"Found {pokemon.Name.ToUpperInvariant()}! 🎉");
            
            // Step 2: Analyze type effectiveness
            ShowLoading($"⚡ Analyzing {pokemon.Name}'s type effectiveness...");
            var analysis = await _typeEffectivenessService.AnalyzeTypeEffectivenessAsync(pokemon);
            
            // Show success message for analysis completed
            ShowSuccess($"Analysis completed for {pokemon.Name.ToUpperInvariant()}! ⚡");
            
            // Step 3: Display results
            await DisplayAnalysisResult(analysis);
        }
        catch (PokemonApiException)
        {
            // Re-throw Pokemon API exceptions to be handled by the calling method
            throw;
        }
        catch (Exception)
        {
            // Handle any other unexpected errors gracefully
            throw new Exception("Failed to analyze Pokemon. Please try again.");
        }
    }

    /// <summary>
    /// Analyzes a Pokemon based on user input
    /// </summary>
    private async Task AnalyzePokemonAsync()
    {
        while (true)
        {
            AnsiConsole.Clear();
            
            // Show a nice header for the analysis
            var header = new Rule("[yellow]POKEMON TYPE ANALYSIS[/]")
                .Centered();
            AnsiConsole.Write(header);
            AnsiConsole.WriteLine();
            
            // Interactive Pokemon selection with quick options
            var rawInput = await SelectPokemonWithQuickOptionsAsync();
            if (string.IsNullOrEmpty(rawInput))
            {
                return; // User chose to return to main menu
            }
            
            // Validate and sanitize input
            var (isValid, sanitizedInput, errorMessage) = ValidatePokemonInput(rawInput);
            
            if (!isValid)
            {
                ShowEnhancedError(errorMessage ?? "Invalid input", new[]
                {
                    "Please enter a valid Pokemon name or ID",
                    "Examples: 'pikachu', 'charizard', '25' (for Pikachu)",
                    "Pokemon names can only contain letters, numbers, and hyphens",
                    "Pokemon names are case-insensitive"
                });
                
                // Ask if user wants to try again
                var retryChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[blue]Would you like to try again?[/]")
                        .AddChoices("🔄 Try Again", "🏠 Return to Main Menu")
                );
                
                if (retryChoice == "🏠 Return to Main Menu")
                {
                    return;
                }
                continue; // Try again
            }
            
            var pokemonName = sanitizedInput;
            
            // Show a brief "preparing" message
            AnsiConsole.MarkupLine("[cyan]🎯 Preparing to analyze...[/]");
            await Task.Delay(500); // Brief pause for UX
            
            try
            {
                await RunAnalysisAsync(pokemonName);
                break; // Success, exit the loop
            }
            catch (PokemonApiException ex)
            {
                // Handle Pokemon API specific errors gracefully
                var suggestions = new[]
                {
                    "Check your internet connection",
                    "Verify the Pokemon name is spelled correctly",
                    "Try using a Pokemon number (e.g., '25' for Pikachu)",
                    "Ensure the Pokemon name exists in the database"
                };
                
                ShowEnhancedError($"Pokemon not found: {ex.Message}", suggestions);
                
                // Ask if user wants to try again
                var retryChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[blue]Would you like to try again?[/]")
                        .AddChoices("🔄 Try Again", "🏠 Return to Main Menu")
                );
                
                if (retryChoice == "🏠 Return to Main Menu")
                {
                    return;
                }
                // Continue loop to try again
            }
                    catch (Exception)
        {
            // Handle any other unexpected errors gracefully
            var suggestions = new[]
            {
                "Check your internet connection",
                "Try again in a few moments",
                "If the problem persists, restart the application"
            };
            
            ShowEnhancedError("Failed to analyze Pokemon", suggestions);
            
            // Ask if user wants to try again
            var retryChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]Would you like to try again?[/]")
                    .AddChoices("🔄 Try Again", "🏠 Return to Main Menu")
            );
            
            if (retryChoice == "🏠 Return to Main Menu")
            {
                return;
            }
            // Continue loop to try again
        }
        }
    }

    /// <summary>
    /// Interactive Pokemon selection with quick options for popular Pokemon
    /// </summary>
    /// <returns>The selected Pokemon name or empty string if user cancels</returns>
    private Task<string> SelectPokemonWithQuickOptionsAsync()
    {
        // Popular Pokemon for quick selection
        var popularPokemon = new[]
        {
            ("Pikachu", "25", "Electric"),
            ("Charizard", "6", "Fire/Flying"),
            ("Blastoise", "9", "Water"),
            ("Venusaur", "3", "Grass/Poison"),
            ("Mewtwo", "150", "Psychic"),
            ("Gyarados", "130", "Water/Flying"),
            ("Lucario", "448", "Fighting/Steel"),
            ("Garchomp", "445", "Dragon/Ground"),
            ("Rayquaza", "384", "Dragon/Flying"),
            ("Arceus", "493", "Normal")
        };

        // Create selection prompt with popular Pokemon
        var choices = new List<string>
        {
            "📝 Enter Pokemon name or ID manually"
        };
        
        // Add popular Pokemon to choices
        choices.AddRange(popularPokemon.Select(p => $"{p.Item1} (#{p.Item2}) - {p.Item3}"));

        // Navigation instructions for Pokemon selection
        AnsiConsole.MarkupLine("[cyan]💡 Use ↑↓ arrow keys to navigate and Enter to select[/]");
        AnsiConsole.WriteLine();

        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[cyan]Choose a Pokemon to analyze:[/]")
                .PageSize(15)
                .AddChoices(choices)
        );

        // Handle different selection types
        if (selection == "📝 Enter Pokemon name or ID manually")
        {
            return Task.FromResult(AnsiConsole.Ask<string>("[cyan]Enter Pokemon name or ID:[/] "));
        }
        else
        {
            // Extract Pokemon name from popular selection
            var pokemonName = selection.Split(' ')[0]; // Get first part (Pokemon name)
            return Task.FromResult(pokemonName);
        }
    }
}