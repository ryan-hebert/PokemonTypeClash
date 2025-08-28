using PokemonTypeClash.Core.Interfaces;
using PokemonTypeClash.Core.Models;

namespace PokemonTypeClash.Console.UI;

/// <summary>
/// Implementation of the console user interface service
/// </summary>
public class ConsoleUI : IConsoleUI
{
    private readonly ITypeEffectivenessService _typeEffectivenessService;
    private readonly IPokemonApiService _pokemonApiService;

    public ConsoleUI(ITypeEffectivenessService typeEffectivenessService, IPokemonApiService pokemonApiService)
    {
        _typeEffectivenessService = typeEffectivenessService;
        _pokemonApiService = pokemonApiService;
    }

    /// <summary>
    /// Displays the main menu to the user with arrow key navigation
    /// </summary>
    /// <returns>The selected menu option (1-4)</returns>
    public string ShowMainMenuWithNavigation()
    {
        int selectedOption = 1;
        const int totalOptions = 4;
        
        while (true)
        {
            System.Console.Clear();
            System.Console.ForegroundColor = ConsoleColor.DarkRed;
            System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⠖⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡤⢤⡀⠀⠀⠀⠀⢸⠀⢱⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠳⡀⠈⠢⡀⠀⠀⢀⠀⠈⡄⠀⠀⠀⠀⠀⠀⠀⡔⠦⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡤⠊⡹⠀⠀⠘⢄⠀⠈⠲⢖⠈⠀⠀⠱⡀⠀⠀⠀⠀⠀⠀⠀⠙⣄⠈⠢⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡠⠖⠁⢠⠞⠀⠀⠀⠀⠘⡄⠀⠀⠀⠀⠀⠀⠀⢱⠀⠀⠀⠀⠀⠀⠀⠀⠈⡆⠀⠀⠉⠑⠢⢄⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⡠⠚⠁⠀⠀⠀⡇⠀⠀⠀⠀⠀⢀⠇⠀⡤⡀⠀⠀⠀⢀⣼⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⢠⣾⣿⣷⣶⣤⣄⣉⠑⣄⠀⠀⠀⠀⠀⠀⠀");
            System.Console.WriteLine("⠀⠀⠀⠀⠀⢀⠞⢁⣴⣾⣿⣿⡆⢇⠀⠀⠀⠀⠀⠸⡀⠀⠂⠿⢦⡰⠀⠀⠋⡄⠀⠀⠀⠀⠀⠀⠀⢰⠁⣿⣿⣿⣿⣿⣿⣿⣿⣷⣌⢆⠀⠀⠀⠀⠀⠀");
            System.Console.WriteLine("⠀⠀⠀⠀⡴⢁⣴⣿⣿⣿⣿⣿⣿⡘⡄⠀⠀⠀⠀⠀⠱⣔⠤⡀⠀⠀⠀⠀⠀⠈⡆⠀⠀⠀⠀⠀⠀⡜⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣮⢣⠀⠀⠀⠀⠀");
            System.Console.WriteLine("⠀⠀⠀⡼⢠⣾⣿⣿⣿⣿⣿⣿⣿⣧⡘⢆⠀⠀⠀⠀⠀⢃⠑⢌⣦⠀⠩⠉⠀⡜⠀⠀⠀⠀⠀⠀⢠⠃⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣧⣣⡀⠀⠀⠀");
            System.Console.WriteLine("⠀⠀⢰⢃⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣦⠱⡀⠀⠀⠀⢸⠀⠀⠓⠭⡭⠙⠋⠀⠀⠀⠀⠀⠀⠀⡜⢰⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⡱⡄⠀⠀");
            System.Console.WriteLine("⠀⠀⡏⣼⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣇⢃⠀⠀⠀⢸⠀⠀⠀⠀⢰⠀⠀⠀⠀⠀⠀⠀⢀⠜⢁⣼⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⠘⣆⠀");
            System.Console.WriteLine("⠀⢸⢱⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡘⣆⠀⠀⡆⠀⠀⠀⠀⠘⡄⠀⠀⠀⠀⡠⠖⣡⣾⠁⣸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣧⢸⠀");
            System.Console.WriteLine("⠀⡏⣾⣿⣿⣿⣿⡿⡛⢟⢿⣿⣿⣿⣿⣿⣿⣧⡈⢦⣠⠃⠀⠀⠀⠀⠀⢱⣀⠤⠒⢉⣾⡉⠻⠋⠈⢘⢿⣿⣿⣿⣿⠿⣿⣿⠏⠉⠻⢿⣿⣿⣿⣿⡘⡆");
            System.Console.WriteLine("⢰⡇⣿⣿⠟⠁⢸⣠⠂⡄⣃⠜⣿⣿⠿⠿⣿⣿⡿⠦⡎⠀⠀⠀⠀⠀⠒⠉⠉⠑⣴⣿⣿⣎⠁⠠⠂⠮⢔⣿⡿⠉⠁⠀⠹⡛⢀⣀⡠⠀⠙⢿⣿⣿⡇⡇");
            System.Console.WriteLine("⠘⡇⠏⠀⠀⠀⡾⠤⡀⠑⠒⠈⠣⣀⣀⡀⠤⠋⢀⡜⣀⣠⣤⣀⠀⠀⠀⠀⠀⠀⠙⢿⡟⠉⡃⠈⢀⠴⣿⣿⣀⡀⠀⠀⠀⠈⡈⠊⠀⠀⠀⠀⠙⢿⡇⡇");
            System.Console.WriteLine("⠀⠿⠀⠀⠀⠀⠈⠀⠉⠙⠓⢤⣀⠀⠁⣀⡠⢔⡿⠊⠀⠀⠀⠀⠙⢦⡀⠀⠐⠢⢄⡀⠁⡲⠃⠀⡜⠀⠹⠟⠻⣿⣰⡐⣄⠎⠀⠀⠀⠀⠀⠀⠀⠀⢣⡇");
            System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠉⠉⠁⠀⡜⠀⠀⠀⠀⠀⠀⠀⠀⠱⡀⠀⠀⠀⠙⢦⣀⢀⡴⠁⠀⠀⠀⠀⠉⠁⢱⠈⢆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢰⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⢱⠀⠀⠀⠀⠈⢏⠉⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠈⡆⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡠⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠱⡄⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⢸⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡜⠀⢹⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⠀⠀⠀⠀⠀⠀⠘⣆⠀⠀⠀⠀⠀⠀⣰⠃⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀");
            System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡾⠀⠀⠘⣆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠸⠁⠀⠀⠀⠀⠀⠀⠸⡄⠀⠀⠀⢀⡴⠁⠀⠀⢀⠇⠀⠀⠀⠀⠀⠀⠀⠀");
            System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢧⠀⠀⠀⠘⢆⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⣧⣠⠤⠖⠋⠀⠀⠀⠀⡸⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠢⡀⠀⠀⠀⠳⢄⠀⠀⠀⠀⠀⠀⠀⢣⠀⠀⠀⠀⠀⠀⠀⠀⡏⠀⠀⠀⠀⠀⠀⢀⡴⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⡠⠊⠈⠁⠀⠀⠀⡔⠛⠲⣤⣀⣀⣀⠀⠈⢣⡀⠀⠀⠀⠀⠀⢸⠁⠀⠀⠀⢀⡠⢔⠝⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠐⢈⠤⠒⣀⠀⠀⠀⠀⣀⠟⠀⠀⠀⠑⠢⢄⡀⠀⠀⠈⡗⠂⠀⠀⠀⠙⢦⠤⠒⢊⡡⠚⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠆⠒⣒⡁⠬⠦⠒⠉⠀⠀⠀⠀⠀⠀⠀⠀⠈⠉⠒⢺⢠⠤⡀⢀⠤⡀⠠⠷⡊⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⠣⡀⡱⠧⡀⢰⠓⠤⡁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠁⠀⠈⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            
            System.Console.WriteLine();
            
            // Enhanced menu styling with Pokemon colors
            System.Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine("║                              PokemonTypeClash                                ║");
            System.Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
            System.Console.ResetColor();
            
            System.Console.WriteLine();
            
            // Menu options with enhanced styling and selection highlighting
            for (int i = 1; i <= totalOptions; i++)
            {
                if (i == selectedOption)
                {
                    System.Console.ForegroundColor = ConsoleColor.Black;
                    System.Console.BackgroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    System.Console.ForegroundColor = ConsoleColor.Yellow;
                    System.Console.BackgroundColor = ConsoleColor.Black;
                }
                
                System.Console.Write("  ");
                
                // Icons for each option
                switch (i)
                {
                    case 1:
                        System.Console.Write("⚡ ");
                        break;
                    case 2:
                        System.Console.Write("📊 ");
                        break;
                    case 3:
                        System.Console.Write("❓ ");
                        break;
                    case 4:
                        System.Console.Write("🚪 ");
                        break;
                }
                
                System.Console.Write($"{i}. ");
                
                // Menu option text
                switch (i)
                {
                    case 1:
                        System.Console.WriteLine("Analyze Pokemon Type Effectiveness");
                        break;
                    case 2:
                        System.Console.WriteLine("View All Pokemon Types");
                        break;
                    case 3:
                        System.Console.WriteLine("Help & Examples");
                        break;
                    case 4:
                        System.Console.WriteLine("Exit");
                        break;
                }
                
                System.Console.WriteLine();
                System.Console.ResetColor();
            }
            
            System.Console.ForegroundColor = ConsoleColor.DarkBlue;
            System.Console.WriteLine("  ──────────────────────────────────────────────────────────────────────────────");
            System.Console.ResetColor();
            System.Console.WriteLine();
            
            // Navigation instructions
            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine("  Use ↑↓ arrow keys to navigate, Enter to select, or type option number (1-4)");
            System.Console.ResetColor();
            
            // Handle user input
            var key = System.Console.ReadKey(true);
            
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    selectedOption = selectedOption > 1 ? selectedOption - 1 : totalOptions;
                    break;
                case ConsoleKey.DownArrow:
                    selectedOption = selectedOption < totalOptions ? selectedOption + 1 : 1;
                    break;
                case ConsoleKey.Enter:
                    return selectedOption.ToString();
                case ConsoleKey.D1:
                case ConsoleKey.D2:
                case ConsoleKey.D3:
                case ConsoleKey.D4:
                case ConsoleKey.NumPad1:
                case ConsoleKey.NumPad2:
                case ConsoleKey.NumPad3:
                case ConsoleKey.NumPad4:
                    return key.KeyChar.ToString();
            }
        }
    }

    /// <summary>
    /// Displays the main menu to the user
    /// </summary>
    public void ShowMainMenu()
    {
        System.Console.Clear();
        
        // Display Pikachu ASCII art
        System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⠖⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
        System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡤⢤⡀⠀⠀⠀⠀⢸⠀⢱⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
        System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠳⡀⠈⠢⡀⠀⠀⢀⠀⠈⡄⠀⠀⠀⠀⠀⠀⠀⡔⠦⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
        System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡤⠊⡹⠀⠀⠘⢄⠀⠈⠲⢖⠈⠀⠀⠱⡀⠀⠀⠀⠀⠀⠀⠀⠙⣄⠈⠢⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
        System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡠⠖⠁⢠⠞⠀⠀⠀⠀⠘⡄⠀⠀⠀⠀⠀⠀⠀⢱⠀⠀⠀⠀⠀⠀⠀⠀⠈⡆⠀⠀⠉⠑⠢⢄⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
        System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⡠⠚⠁⠀⠀⠀⡇⠀⠀⠀⠀⠀⢀⠇⠀⡤⡀⠀⠀⠀⢀⣼⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⢠⣾⣿⣷⣶⣤⣄⣉⠑⣄⠀⠀⠀⠀⠀⠀⠀");
        System.Console.WriteLine("⠀⠀⠀⠀⠀⢀⠞⢁⣴⣾⣿⣿⡆⢇⠀⠀⠀⠀⠀⠸⡀⠀⠂⠿⢦⡰⠀⠀⠋⡄⠀⠀⠀⠀⠀⠀⠀⢰⠁⣿⣿⣿⣿⣿⣿⣿⣿⣷⣌⢆⠀⠀⠀⠀⠀⠀");
        System.Console.WriteLine("⠀⠀⠀⠀⡴⢁⣴⣿⣿⣿⣿⣿⣿⡘⡄⠀⠀⠀⠀⠀⠱⣔⠤⡀⠀⠀⠀⠀⠀⠈⡆⠀⠀⠀⠀⠀⠀⡜⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣮⢣⠀⠀⠀⠀⠀");
        System.Console.WriteLine("⠀⠀⠀⡼⢠⣾⣿⣿⣿⣿⣿⣿⣿⣧⡘⢆⠀⠀⠀⠀⠀⢃⠑⢌⣦⠀⠩⠉⠀⡜⠀⠀⠀⠀⠀⠀⢠⠃⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣧⣣⡀⠀⠀⠀");
        System.Console.WriteLine("⠀⠀⢰⢃⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣦⠱⡀⠀⠀⠀⢸⠀⠀⠓⠭⡭⠙⠋⠀⠀⠀⠀⠀⠀⠀⡜⢰⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⡱⡄⠀⠀");
        System.Console.WriteLine("⠀⠀⡏⣼⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣇⢃⠀⠀⠀⢸⠀⠀⠀⠀⢰⠀⠀⠀⠀⠀⠀⠀⢀⠜⢁⣼⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⠘⣆⠀");
        System.Console.WriteLine("⠀⢸⢱⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡘⣆⠀⠀⡆⠀⠀⠀⠀⠘⡄⠀⠀⠀⠀⡠⠖⣡⣾⠁⣸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣧⢸⠀");
        System.Console.WriteLine("⠀⡏⣾⣿⣿⣿⣿⡿⡛⢟⢿⣿⣿⣿⣿⣿⣿⣧⡈⢦⣠⠃⠀⠀⠀⠀⠀⢱⣀⠤⠒⢉⣾⡉⠻⠋⠈⢘⢿⣿⣿⣿⣿⠿⣿⣿⠏⠉⠻⢿⣿⣿⣿⣿⡘⡆");
        System.Console.WriteLine("⢰⡇⣿⣿⠟⠁⢸⣠⠂⡄⣃⠜⣿⣿⠿⠿⣿⣿⡿⠦⡎⠀⠀⠀⠀⠀⠒⠉⠉⠑⣴⣿⣿⣎⠁⠠⠂⠮⢔⣿⡿⠉⠁⠀⠹⡛⢀⣀⡠⠀⠙⢿⣿⣿⡇⡇");
        System.Console.WriteLine("⠘⡇⠏⠀⠀⠀⡾⠤⡀⠑⠒⠈⠣⣀⣀⡀⠤⠋⢀⡜⣀⣠⣤⣀⠀⠀⠀⠀⠀⠀⠙⢿⡟⠉⡃⠈⢀⠴⣿⣿⣀⡀⠀⠀⠀⠈⡈⠊⠀⠀⠀⠀⠙⢿⡇⡇");
        System.Console.WriteLine("⠀⠿⠀⠀⠀⠀⠈⠀⠉⠙⠓⢤⣀⠀⠁⣀⡠⢔⡿⠊⠀⠀⠀⠀⠙⢦⡀⠀⠐⠢⢄⡀⠁⡲⠃⠀⡜⠀⠹⠟⠻⣿⣰⡐⣄⠎⠀⠀⠀⠀⠀⠀⠀⠀⢣⡇");
        System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠉⠉⠁⠀⡜⠀⠀⠀⠀⠀⠀⠀⠀⠱⡀⠀⠀⠀⠙⢦⣀⢀⡴⠁⠀⠀⠀⠀⠉⠁⢱⠈⢆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
        System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢰⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⢱⠀⠀⠀⠀⠈⢏⠉⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠈⡆⠀⠀⠀⠀⠀⠀⠀⠀⠀");
        System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡠⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠱⡄⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⢸⠀⠀⠀⠀⠀⠀⠀⠀⠀");
        System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡜⠀⢹⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⠀⠀⠀⠀⠀⠀⠘⣆⠀⠀⠀⠀⠀⠀⣰⠃⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀");
        System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡾⠀⠀⠘⣆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠸⠁⠀⠀⠀⠀⠀⠀⠸⡄⠀⠀⠀⢀⡴⠁⠀⠀⢀⠇⠀⠀⠀⠀⠀⠀⠀⠀");
        System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢧⠀⠀⠀⠘⢆⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⣧⣠⠤⠖⠋⠀⠀⠀⠀⡸⠀⠀⠀⠀⠀⠀⠀⠀⠀");
        System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠢⡀⠀⠀⠀⠳⢄⠀⠀⠀⠀⠀⠀⠀⢣⠀⠀⠀⠀⠀⠀⠀⠀⡏⠀⠀⠀⠀⠀⠀⢀⡴⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀");
        System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⡠⠊⠈⠁⠀⠀⠀⡔⠛⠲⣤⣀⣀⣀⠀⠈⢣⡀⠀⠀⠀⠀⠀⢸⠁⠀⠀⠀⢀⡠⢔⠝⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
        System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠐⢈⠤⠒⣀⠀⠀⠀⠀⣀⠟⠀⠀⠀⠑⠢⢄⡀⠀⠀⠈⡗⠂⠀⠀⠀⠙⢦⠤⠒⢊⡡⠚⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
        System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠆⠒⣒⡁⠬⠦⠒⠉⠀⠀⠀⠀⠀⠀⠀⠀⠈⠉⠒⢺⢠⠤⡀⢀⠤⡀⠠⠷⡊⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
        System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⠣⡀⡱⠧⡀⢰⠓⠤⡁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
        System.Console.WriteLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠁⠀⠈⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
        
        System.Console.WriteLine();
        
        // Enhanced menu styling with Pokemon colors
        System.Console.ForegroundColor = ConsoleColor.Blue;
        System.Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine("║                              PokemonTypeClash                                ║");
        System.Console.ForegroundColor = ConsoleColor.Blue;
        System.Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
        System.Console.ResetColor();
        
        System.Console.WriteLine();
        
        // Menu options with enhanced styling
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.Write("  ⚡ ");
        System.Console.ResetColor();
        System.Console.Write("1. ");
        System.Console.ForegroundColor = ConsoleColor.White;
        System.Console.WriteLine("Analyze Pokemon Type Effectiveness");
        System.Console.WriteLine();
        System.Console.ResetColor();
        
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.Write("  📊 ");
        System.Console.ResetColor();
        System.Console.Write("2. ");
        System.Console.ForegroundColor = ConsoleColor.White;
        System.Console.WriteLine("View All Pokemon Types");
        System.Console.WriteLine();
        System.Console.ResetColor();
        
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.Write("  ❓ ");
        System.Console.ResetColor();
        System.Console.Write("3. ");
        System.Console.ForegroundColor = ConsoleColor.White;
        System.Console.WriteLine("Help & Examples");
        System.Console.WriteLine();
        System.Console.ResetColor();
        
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.Write("  🚪 ");
        System.Console.ResetColor();
        System.Console.Write("4. ");
        System.Console.ForegroundColor = ConsoleColor.White;
        System.Console.WriteLine("Exit");
        System.Console.ResetColor();
        
        System.Console.WriteLine();
        System.Console.ForegroundColor = ConsoleColor.DarkBlue;
        System.Console.WriteLine("  ──────────────────────────────────────────────────────────────────────────────");
        System.Console.ResetColor();
        System.Console.WriteLine();
    }

    /// <summary>
    /// Gets user input with a prompt
    /// </summary>
    /// <param name="prompt">The prompt to display to the user</param>
    /// <returns>The user's input</returns>
    public string GetUserInput(string prompt)
    {
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.Write($"  🎯 {prompt}: ");
        System.Console.ResetColor();
        return System.Console.ReadLine()?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// Displays the type analysis result to the user
    /// </summary>
    /// <param name="result">The analysis result to display</param>
    public void DisplayAnalysisResult(TypeAnalysisResult result)
    {
        System.Console.Clear();
        
        // Enhanced header with Pokemon styling
        System.Console.ForegroundColor = ConsoleColor.Blue;
        System.Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine($"║                                   TYPE ANALYSIS                              ║");
        System.Console.ForegroundColor = ConsoleColor.Blue;
        System.Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
        System.Console.ResetColor();
        System.Console.WriteLine();
        
        // Display Pokemon with Pokemon-specific colors (Pikachu yellow for Pokemon icon)
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.Write("  🐶 ");
        System.Console.ForegroundColor = ConsoleColor.White;
        System.Console.WriteLine($"{result.Pokemon.Name.ToUpperInvariant()}");
        System.Console.WriteLine();
        
        // Display Pokemon types with Pokemon type-specific colors (Pokemon Red/Blue theme)
        System.Console.ForegroundColor = ConsoleColor.DarkRed;
        System.Console.Write("  ⚡ Types: ");
        System.Console.ForegroundColor = ConsoleColor.White;
        var typeNames = string.Join(", ", result.Pokemon.Types.Select(t => char.ToUpperInvariant(t.Name[0]) + t.Name[1..]));
        System.Console.WriteLine(typeNames);
        System.Console.ResetColor();
        System.Console.WriteLine();

        // Display analysis results in a table format
        System.Console.ForegroundColor = ConsoleColor.Blue;
        System.Console.WriteLine("  ┌─────────────────────────────────────────────────────────────────────────────┐");
        System.Console.WriteLine("  │                           TYPE EFFECTIVENESS TABLE                          │");
        System.Console.WriteLine("  ├─────────────────────────────────────────────────────────────────────────────┤");
        
        // Offensive Analysis Section
        System.Console.WriteLine("  │  OFFENSIVE CAPABILITIES                                                     │");
        System.Console.WriteLine("  ├─────────────────────────────────────────────────────────────────────────────┤");
        
        // Strong Against
        if (result.StrongAgainst.Any())
        {
            System.Console.Write("  │  ");
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.Write("⚔️  Strong Against: ");
            var strongTypes = string.Join(", ", result.StrongAgainst.Select(t => char.ToUpperInvariant(t.Name[0]) + t.Name[1..]));
            var padding = 56 - strongTypes.Length;
            System.Console.Write(strongTypes);
            System.Console.Write(new string(' ', Math.Max(0, padding)));
            System.Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.WriteLine("│");
        }
        
        // Weak Against
        if (result.WeakAgainst.Any())
        {
            System.Console.Write("  │  ");
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.Write("⚠️  Weak Against:   ");
            var weakTypes = string.Join(", ", result.WeakAgainst.Select(t => char.ToUpperInvariant(t.Name[0]) + t.Name[1..]));
            var padding = 56 - weakTypes.Length;
            System.Console.Write(weakTypes);
            System.Console.Write(new string(' ', Math.Max(0, padding)));
            System.Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.WriteLine("│");
        }
        
        // No Effect Against
        if (result.NoEffectAgainst.Any())
        {
            System.Console.Write("  │  ");
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.Write("❌ No Effect:      ");
            var noEffectTypes = string.Join(", ", result.NoEffectAgainst.Select(t => char.ToUpperInvariant(t.Name[0]) + t.Name[1..]));
            var padding = 56 - noEffectTypes.Length;
            System.Console.Write(noEffectTypes);
            System.Console.Write(new string(' ', Math.Max(0, padding)));
            System.Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.WriteLine("│");
        }
        
        // If no offensive data, show "None"
        if (!result.StrongAgainst.Any() && !result.WeakAgainst.Any() && !result.NoEffectAgainst.Any())
        {
            System.Console.Write("  │  No offensive data available");
            System.Console.WriteLine("                                    │");
        }
        
        System.Console.WriteLine("  ├─────────────────────────────────────────────────────────────────────────────┤");
        
        // Defensive Analysis Section
        System.Console.WriteLine("  │   DEFENSIVE CAPABILITIES                                                    │");
        System.Console.WriteLine("  ├─────────────────────────────────────────────────────────────────────────────┤");
        
        // Immune To
        if (result.ImmuneTo.Any())
        {
            System.Console.Write("  │  ");
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.Write("🛡️  Immune To:     ");
            var immuneTypes = string.Join(", ", result.ImmuneTo.Select(t => char.ToUpperInvariant(t.Name[0]) + t.Name[1..]));
            var padding = 56 - immuneTypes.Length;
            System.Console.Write(immuneTypes);
            System.Console.Write(new string(' ', Math.Max(0, padding)));
            System.Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.WriteLine("│");
        }
        
        // Resistant To
        if (result.ResistantTo.Any())
        {
            System.Console.Write("  │  ");
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.Write("🛡️  Resistant To:  ");
            var resistantTypes = string.Join(", ", result.ResistantTo.Select(t => char.ToUpperInvariant(t.Name[0]) + t.Name[1..]));
            var padding = 57 - resistantTypes.Length;
            System.Console.Write(resistantTypes);
            System.Console.Write(new string(' ', Math.Max(0, padding)));
            System.Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.WriteLine("│");
        }
        
        // Vulnerable To
        if (result.VulnerableTo.Any())
        {
            System.Console.Write("  │  ");
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.Write("⚠️  Vulnerable To: ");
            var vulnerableTypes = string.Join(", ", result.VulnerableTo.Select(t => char.ToUpperInvariant(t.Name[0]) + t.Name[1..]));
            var padding = 57 - vulnerableTypes.Length;
            System.Console.Write(vulnerableTypes);
            System.Console.Write(new string(' ', Math.Max(0, padding)));
            System.Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.WriteLine("│");
        }
        
        // If no defensive data, show "None"
        if (!result.ImmuneTo.Any() && !result.ResistantTo.Any() && !result.VulnerableTo.Any())
        {
            System.Console.Write("  │  No defensive data available");
            System.Console.WriteLine("                                    │");
        }
        
        System.Console.WriteLine("  └─────────────────────────────────────────────────────────────────────────────┘");
        System.Console.ResetColor();

        // Footer with timestamp
        System.Console.ForegroundColor = ConsoleColor.DarkGray;
        System.Console.WriteLine("  ──────────────────────────────────────────────────────────────────────────────");
        System.Console.WriteLine($"  Analysis completed at: {result.AnalysisTimestamp:yyyy-MM-dd HH:mm:ss}");
        System.Console.ResetColor();
        System.Console.WriteLine();
    }

    /// <summary>
    /// Displays an error message to the user
    /// </summary>
    /// <param name="message">The error message to display</param>
    public void ShowError(string message)
    {
        System.Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine($"  ❌ Error: {message}");
        System.Console.ResetColor();
        System.Console.WriteLine();
    }

    /// <summary>
    /// Displays a success message to the user
    /// </summary>
    /// <param name="message">The success message to display</param>
    public void ShowSuccess(string message)
    {
        System.Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine($"  ✅ {message}");
        System.Console.ResetColor();
        System.Console.WriteLine();
    }

    /// <summary>
    /// Displays help information and examples
    /// </summary>
    public void ShowHelp()
    {
        System.Console.Clear();
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine("=== HELP & EXAMPLES ===");
        System.Console.ResetColor();
        System.Console.WriteLine();
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.WriteLine("PokemonTypeClash analyzes the type effectiveness of Pokemon.");
        System.Console.WriteLine("You can enter any Pokemon name or ID to get a detailed analysis.");
        System.Console.ResetColor();
        System.Console.WriteLine();
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine("Examples:");
        System.Console.ResetColor();
        System.Console.WriteLine("- pikachu (Electric type)");
        System.Console.WriteLine("- charizard (Fire/Flying dual type)");
        System.Console.WriteLine("- 25 (Pokemon ID for Pikachu)");
        System.Console.WriteLine("- 6 (Pokemon ID for Charizard)");
        System.Console.WriteLine();
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine("The analysis shows:");
        System.Console.ResetColor();
        System.Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine("✓ Strong Against: Types this Pokemon's attacks are super effective against");
        System.Console.ResetColor();
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine("- Weak Against: Types this Pokemon's attacks are not very effective against");
        // No Effect Against
        System.Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine("✗ No Effect Against: Types this Pokemon's attacks have no effect on");
        System.Console.ResetColor();
        // Immune To
        System.Console.ResetColor();
        System.Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine("✓ Immune To: Types this Pokemon takes no damage from");
        System.Console.ResetColor();
        // Resistant To
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine("- Resistant To: Types this Pokemon takes reduced damage from");
        System.Console.ResetColor();
        System.Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine("✗ Vulnerable To: Types this Pokemon takes super effective damage from");
        System.Console.ResetColor();
        System.Console.WriteLine();
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.WriteLine("Press any key to continue...");
        System.Console.ResetColor();
        System.Console.ReadKey();
    }

    /// <summary>
    /// Displays a loading message
    /// </summary>
    /// <param name="message">The loading message to display</param>
    public void ShowLoading(string message)
    {
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.Write($"  ⏳ {message}... ");
        System.Console.ResetColor();
    }

    /// <summary>
    /// Shows an animated loading spinner while performing an async operation
    /// </summary>
    /// <param name="message">The loading message to display</param>
    /// <param name="operation">The async operation to perform</param>
    /// <returns>The result of the operation</returns>
    public async Task<T> ShowLoadingAnimationAsync<T>(string message, Func<Task<T>> operation)
    {
        var spinner = new[] { "⠋", "⠙", "⠹", "⠸", "⠼", "⠴", "⠦", "⠧", "⠇", "⠏" };
        var spinnerIndex = 0;
        
        // Start the operation
        var operationTask = operation();
        
        // Show animated spinner while operation is running
        while (!operationTask.IsCompleted)
        {
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.Write($"\r  {spinner[spinnerIndex]} {message}... ");
            System.Console.ResetColor();
            
            await Task.Delay(100); // Update spinner every 100ms
            spinnerIndex = (spinnerIndex + 1) % spinner.Length;
        }
        
        // Clear the loading line
        System.Console.Write("\r" + new string(' ', message.Length + 20) + "\r");
        
        // Show completion message
        System.Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine($"  ✅ {message} completed!");
        System.Console.ResetColor();
        
        return await operationTask;
    }

    /// <summary>
    /// Clears the loading message
    /// </summary>
    public void ClearLoading()
    {
        System.Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine("Done! ✨");
        System.Console.ResetColor();
    }

    /// <summary>
    /// Prompts the user to continue or exit
    /// </summary>
    /// <returns>True if the user wants to continue, false to exit</returns>
    public bool AskToContinue()
    {
        var response = GetUserInput("Would you like to analyze another Pokemon? (y/n)");
        return response.Equals("y", StringComparison.OrdinalIgnoreCase) || 
               response.Equals("yes", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Runs the main application loop
    /// </summary>
    public async Task RunAsync()
    {
        bool continueRunning = true;

        while (continueRunning)
        {
            var choice = ShowMainMenuWithNavigation();

            switch (choice)
            {
                case "1":
                    await AnalyzePokemonAsync();
                    break;
                case "2":
                    ShowAllTypes();
                    break;
                case "3":
                    ShowHelp();
                    break;
                case "4":
                    continueRunning = false;
                    break;
                default:
                    ShowError("Invalid option. Please choose 1-4.");
                    System.Console.WriteLine("Press any key to continue...");
                    System.Console.ReadKey();
                    break;
            }
        }

        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine("Thank you for using PokemonTypeClash!");
        System.Console.ResetColor();
    }

    /// <summary>
    /// Analyzes a Pokemon based on user input
    /// </summary>
    private async Task AnalyzePokemonAsync()
    {
        bool continueAnalyzing = true;
        
        while (continueAnalyzing)
        {
            try
            {
                var pokemonName = GetUserInput("Enter Pokemon name or ID");
                
                if (string.IsNullOrWhiteSpace(pokemonName))
                {
                    ShowError("Pokemon name cannot be empty.");
                    continue;
                }

                // Use animated loading for API calls
                var pokemon = await ShowLoadingAnimationAsync(
                    "Retrieving Pokemon data from PokéAPI", 
                    () => _pokemonApiService.GetPokemonAsync(pokemonName)
                );

                var result = await ShowLoadingAnimationAsync(
                    "Analyzing type effectiveness", 
                    () => _typeEffectivenessService.AnalyzeTypeEffectivenessAsync(pokemon)
                );

                DisplayAnalysisResult(result);

                continueAnalyzing = AskToContinue();
            }
            catch (Exception ex)
            {
                ShowError($"Failed to analyze Pokemon: {ex.Message}");
                System.Console.WriteLine("Please try again with a valid Pokemon name or ID.");
                System.Console.WriteLine();
                // Continue the loop to allow user to try another Pokemon
                continue;
            }
        }
    }

    /// <summary>
    /// Shows all available Pokemon types
    /// </summary>
    private void ShowAllTypes()
    {
        System.Console.Clear();
        
        // Enhanced header with Pokemon styling
        System.Console.ForegroundColor = ConsoleColor.Blue;
        System.Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine("║                       POKEMON TYPE COMPENDIUM                  ║");
        System.Console.ForegroundColor = ConsoleColor.Blue;
        System.Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");
        System.Console.ResetColor();
        System.Console.WriteLine();
        
        var types = new[]
        {
            "Normal", "Fighting", "Flying", "Poison", "Ground", "Rock",
            "Bug", "Ghost", "Steel", "Fire", "Water", "Grass",
            "Electric", "Psychic", "Ice", "Dragon", "Dark", "Fairy"
        };

        // Type colors mapping with enhanced colors
        var typeColors = new Dictionary<string, ConsoleColor>
        {
            {"Normal", ConsoleColor.White},
            {"Fighting", ConsoleColor.Red},
            {"Flying", ConsoleColor.Cyan},
            {"Poison", ConsoleColor.Magenta},
            {"Ground", ConsoleColor.Yellow},
            {"Rock", ConsoleColor.DarkGray},
            {"Bug", ConsoleColor.Green},
            {"Ghost", ConsoleColor.DarkMagenta},
            {"Steel", ConsoleColor.Gray},
            {"Fire", ConsoleColor.Red},
            {"Water", ConsoleColor.Blue},
            {"Grass", ConsoleColor.Green},
            {"Electric", ConsoleColor.Yellow},
            {"Psychic", ConsoleColor.Magenta},
            {"Ice", ConsoleColor.Cyan},
            {"Dragon", ConsoleColor.DarkMagenta},
            {"Dark", ConsoleColor.DarkGray},
            {"Fairy", ConsoleColor.Magenta}
        };

        // Type icons for visual enhancement (using consistent bullet style)
        var typeIcons = new Dictionary<string, string>
        {
            {"Normal", "●"},
            {"Fighting", "●"},
            {"Flying", "●"},
            {"Poison", "●"},
            {"Ground", "●"},
            {"Rock", "●"},
            {"Bug", "●"},
            {"Ghost", "●"},
            {"Steel", "●"},
            {"Fire", "●"},
            {"Water", "●"},
            {"Grass", "●"},
            {"Electric", "●"},
            {"Psychic", "●"},
            {"Ice", "●"},
            {"Dragon", "●"},
            {"Dark", "●"},
            {"Fairy", "●"}
        };

        // Create sophisticated table header
        System.Console.ForegroundColor = ConsoleColor.DarkBlue;
        System.Console.WriteLine("┌────────────────────┬────────────────────┬──────────────────────┐");
        System.Console.WriteLine("│      TYPE NAME     │      TYPE NAME     │      TYPE NAME       │");
        System.Console.WriteLine("├────────────────────┼────────────────────┼──────────────────────┤");
        System.Console.ResetColor();

        // Display types in a 3-column table format with proper alignment
        for (int i = 0; i < types.Length; i += 3)
        {
            // Left column
            if (i < types.Length)
            {
                var type = types[i];
                var icon = typeIcons.ContainsKey(type) ? typeIcons[type] : "●";
                var color = typeColors.ContainsKey(type) ? typeColors[type] : ConsoleColor.White;
                
                System.Console.ForegroundColor = ConsoleColor.DarkBlue;
                System.Console.Write("│ ");
                System.Console.ForegroundColor = color;
                System.Console.Write($"{icon} {type}");
                System.Console.ForegroundColor = ConsoleColor.DarkBlue;
                System.Console.Write(new string(' ', 16 - type.Length));
                System.Console.Write(" │ ");
            }
            else
            {
                System.Console.ForegroundColor = ConsoleColor.DarkBlue;
                System.Console.Write("│                     │ ");
            }

            // Middle column
            if (i + 1 < types.Length)
            {
                var type = types[i + 1];
                var icon = typeIcons.ContainsKey(type) ? typeIcons[type] : "●";
                var color = typeColors.ContainsKey(type) ? typeColors[type] : ConsoleColor.White;
                
                System.Console.ForegroundColor = color;
                System.Console.Write($"{icon} {type}");
                System.Console.ForegroundColor = ConsoleColor.DarkBlue;
                System.Console.Write(new string(' ', 16 - type.Length));
                System.Console.Write(" │ ");
            }
            else
            {
                System.Console.ForegroundColor = ConsoleColor.DarkBlue;
                System.Console.Write("                     │ ");
            }

            // Right column
            if (i + 2 < types.Length)
            {
                var type = types[i + 2];
                var icon = typeIcons.ContainsKey(type) ? typeIcons[type] : "●";
                var color = typeColors.ContainsKey(type) ? typeColors[type] : ConsoleColor.White;
                
                System.Console.ForegroundColor = color;
                System.Console.Write($"{icon} {type}");
                System.Console.ForegroundColor = ConsoleColor.DarkBlue;
                System.Console.Write(new string(' ', 18 - type.Length));
                System.Console.WriteLine(" │");
            }
            else
            {
                System.Console.ForegroundColor = ConsoleColor.DarkBlue;
                System.Console.WriteLine("                     │");
            }
        }

        // Table footer
        System.Console.ForegroundColor = ConsoleColor.DarkBlue;
        System.Console.WriteLine("└────────────────────┴────────────────────┴──────────────────────┘");
        System.Console.ResetColor();
        
        System.Console.WriteLine();
        
        // Enhanced footer with statistics
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.WriteLine($"  📊 Total Pokemon Types: {types.Length}");
        System.Console.WriteLine("  🎨 Each type has unique strengths and weaknesses in battle!");
        System.Console.ResetColor();
        System.Console.WriteLine();
        
        // Navigation prompt
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.Write("  Press any key to return to the main menu... ");
        System.Console.ResetColor();
        System.Console.ReadKey();
    }
}
