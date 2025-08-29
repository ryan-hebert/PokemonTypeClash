# PokemonTypeClash 🎮

A fun console game that analyzes Pokémon type effectiveness! Enter any Pokémon name and see their strengths, weaknesses, and battle strategies with beautiful, cross-platform UI powered by Spectre.Console.

## ✨ What's New in v2.0!

🎨 **Beautiful Cross-Platform UI** - Consistent, modern interface across Windows, macOS, and Linux  
🎯 **Interactive Navigation** - Arrow key navigation with visual feedback  
📊 **Rich Data Tables** - Beautifully formatted type effectiveness data  
⚡ **Loading Animations** - Smooth progress indicators during API calls  
🛡️ **Enhanced Error Handling** - User-friendly error messages and recovery suggestions  
🎮 **Quick Pokemon Selection** - Popular Pokemon available for instant analysis  
🧹 **Clean Architecture** - Streamlined codebase with single UI implementation  

## 🚀 Quick Start - Play in 3 Steps!

### 1. Clone the Repository
```bash
git clone https://github.com/ryan-hebert/PokemonTypeClash.git
cd PokemonTypeClash
```

### 2. Run the Game

**On Windows:**
```cmd
run.bat
```

**On macOS/Linux:**
```bash
./run.sh
```

**Alternative (any platform):**
```bash
dotnet run --project src/PokemonTypeClash.Console
```

### 3. Start Playing! 🎮
- Use **↑↓ arrow keys** to navigate menus
- Choose "Analyze Pokemon Type Effectiveness"
- Select from popular Pokemon or enter any name
- View detailed battle analysis with beautiful formatting!

## 🎯 What You'll See

The game provides comprehensive Pokemon battle analysis with a modern, cross-platform interface:

- **⚔️ Offensive Capabilities**: What types your Pokemon is strong against
- **🛡️ Defensive Weaknesses**: What types can exploit your Pokemon
- **💪 Resistances**: What attacks your Pokemon can shrug off
- **🚫 Immunities**: What attacks have no effect on your Pokemon
- **📊 Damage Multipliers**: Exact effectiveness calculations
- **🎨 Beautiful Tables**: Rich, color-coded data presentation
- **⚡ Smooth Animations**: Loading indicators and progress feedback

## 🖥️ System Requirements

- **.NET 9.0 SDK** - [Download here](https://dotnet.microsoft.com/download)
- **Windows 10/11, macOS 10.15+, or Linux**
- **4GB RAM** (minimum)
- **Internet connection** (for Pokemon data)
- **Color terminal support** (for best experience)

## 🔧 Installation Troubleshooting

### Windows Users
If you get a bash error, use the Windows batch file:
```cmd
run.bat
```

**If you don't have .NET installed:**
1. Install .NET 9.0 SDK from [Microsoft](https://dotnet.microsoft.com/download)
2. Run the installer
3. Restart your command prompt
4. Try `run.bat` again

### macOS Users
Make sure the script is executable:
```bash
chmod +x run.sh
./run.sh
```

### Linux Users
Make sure the script is executable:
```bash
chmod +x run.sh
./run.sh
```

### All Platforms
If you have issues, try the direct .NET command:
```bash
dotnet run --project src/PokemonTypeClash.Console
```


## 🎯 Tips for Players

- **Type names are case-insensitive**: "Pikachu", "pikachu", or "PIKACHU" all work
- **Use Pokemon names or numbers**: Try "25" instead of "pikachu"
- **Quick Selection**: Choose from popular Pokemon for instant analysis
- **Arrow Key Navigation**: Use ↑↓ keys to navigate all menus
- **Explore different Pokemon**: Each has unique type combinations
- **Study the effectiveness**: Learn which types counter others
- **Use the help menu**: Press "3" for examples and tips

## 🚀 Coming Soon!

**Standalone Executables** - We're working on single-file, self-contained executables that won't require .NET installation:

- **Windows**: `PokemonTypeClash.Console.exe` (~50MB)
- **macOS**: `PokemonTypeClash.Console` (Intel & Apple Silicon)
- **Linux**: `PokemonTypeClash.Console` (~50MB)

These will be available for download from [GitHub Releases](https://github.com/ryan-hebert/PokemonTypeClash/releases) once ready!

## 🏗️ For Developers

### Build and Test
```bash
# Build the solution (clean release build)
dotnet build -c Release

# Run all tests
dotnet test

# Run specific test project
dotnet test tests/PokemonTypeClash.Core.Tests
```

### Create Self-Contained Executables
```bash
# Build single-file executable (for development)
dotnet publish src/PokemonTypeClash.Console -c Release -o ./my-app --self-contained -r win-x64 -p:PublishSingleFile=true
```

### Project Structure
```
src/
├── PokemonTypeClash.Core/          # Domain models and interfaces
├── PokemonTypeClash.Application/   # Business logic and services
├── PokemonTypeClash.Infrastructure/# API clients and data access
└── PokemonTypeClash.Console/       # Spectre.Console UI implementation

tests/
├── PokemonTypeClash.Core.Tests/
├── PokemonTypeClash.Application.Tests/
├── PokemonTypeClash.Infrastructure.Tests/
└── PokemonTypeClash.Performance.Tests/
```

### Key Technologies
- **.NET 9.0** - Modern, cross-platform framework
- **Spectre.Console** - Beautiful console UI library
- **PokéAPI** - Comprehensive Pokemon data
- **Dependency Injection** - Clean, testable architecture
- **xUnit** - Comprehensive test coverage (39 tests)

### Architecture Highlights
- **Clean Architecture** - Separation of concerns with clear layers
- **Single UI Implementation** - Streamlined Spectre.Console-based interface
- **Zero Build Warnings** - Production-ready, clean builds
- **Cross-Platform Compatibility** - Consistent experience across all platforms
- **Graceful Error Handling** - User-friendly error messages and recovery

## 🐛 Troubleshooting

### Common Issues

**"Command not found: dotnet"**
- Install .NET 9.0 SDK from [Microsoft](https://dotnet.microsoft.com/download)

**"Permission denied" on run.sh**
- Run: `chmod +x run.sh`

**"No such file or directory" on Windows**
- Use `run.bat` instead of `run.sh`

**API errors or timeouts**
- Check your internet connection
- The app will retry automatically with user-friendly error messages

**Build errors**
- Make sure you have .NET 9.0 SDK installed
- Try: `dotnet clean && dotnet build -c Release`

**UI rendering issues**
- Ensure your terminal supports colors and Unicode
- Try a different terminal application if needed

## 📚 Learn More

- [Pokemon Type Chart](https://pokemondb.net/type)
- [PokéAPI Documentation](https://pokeapi.co/docs/v2)
- [Spectre.Console Documentation](https://spectreconsole.net/)
- [.NET Documentation](https://docs.microsoft.com/dotnet/)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Ensure clean builds with `dotnet build -c Release`
6. Submit a pull request

### Development Guidelines
- **Clean Code** - Follow SOLID principles and clean architecture
- **Test Coverage** - Maintain comprehensive test coverage
- **Zero Warnings** - Ensure builds are clean with no warnings
- **Cross-Platform** - Test on multiple platforms when possible
- **User Experience** - Prioritize user-friendly error handling and feedback


**Ready to become a Pokemon master? Start analyzing types now! ⚡** 

*Experience the beautiful, cross-platform UI powered by Spectre.Console! 🎨* 
