# PokemonTypeClash 🎮

A fun console game that analyzes Pokémon type effectiveness! Enter any Pokémon name and see their strengths, weaknesses, and battle strategies with authentic Pokemon-themed styling.

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

**🎁 No .NET? No Problem!**
- Download pre-built executables from [Releases](https://github.com/ryan-hebert/PokemonTypeClash/releases)
- **Single-file builds** (~50-80MB) for clean, compact downloads
- **ARM64 support** for Apple Silicon (M1/M2) Macs
- **Ready to run** - Proper permissions set for Linux/macOS
- Extract and run - no .NET installation required!

### 3. Start Playing! 🎮
- Use arrow keys or number keys to navigate
- Choose "Analyze Pokemon Type Effectiveness"
- Enter any Pokemon name (like "pikachu", "charizard", "mewtwo")
- View detailed battle analysis!

## 🎯 What You'll See

The game provides comprehensive Pokemon battle analysis including:

- **⚔️ Offensive Capabilities**: What types your Pokemon is strong against
- **🛡️ Defensive Weaknesses**: What types can exploit your Pokemon
- **💪 Resistances**: What attacks your Pokemon can shrug off
- **🚫 Immunities**: What attacks have no effect on your Pokemon
- **📊 Damage Multipliers**: Exact effectiveness calculations

## 🖥️ System Requirements

- **.NET 9.0 SDK** - [Download here](https://dotnet.microsoft.com/download)
- **Windows 10/11, macOS 10.15+, or Linux**
- **4GB RAM** (minimum)
- **Internet connection** (for Pokemon data)

**🎁 Alternative: Download pre-built executables**
- No .NET installation required!
- Available for Windows, macOS (Intel/ARM), and Linux
- **Single-file builds** (~50-80MB) for clean downloads
- **Ready to run** - Executable permissions already set
- Download from [GitHub Releases](https://github.com/ryan-hebert/PokemonTypeClash/releases)

## 🔧 Installation Troubleshooting

### Windows Users
If you get a bash error, use the Windows batch file:
```cmd
run.bat
```

**If you don't have .NET installed:**
1. **Option A**: Install .NET 9.0 SDK from [Microsoft](https://dotnet.microsoft.com/download)
2. **Option B**: Download pre-built executable from [Releases](https://github.com/ryan-hebert/PokemonTypeClash/releases)
   - Download `PokemonTypeClash-win-x64.zip`
3. Run the installer
4. Restart your command prompt
5. Try `run.bat` again

### macOS Users
Make sure the script is executable:
```bash
chmod +x run.sh
./run.sh
```

**For Apple Silicon (M1/M2) Macs:**
- Download `PokemonTypeClash-osx-arm64.tar.gz` for native ARM64 performance
- Download `PokemonTypeClash-osx-x64.tar.gz` for Intel compatibility

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

## 🎮 Example Gameplay

```
╔══════════════════════════════════════════════════════════════════════════════╗
║                              PokemonTypeClash                                ║
╚══════════════════════════════════════════════════════════════════════════════╝

  🐶 PIKACHU

  ⚡ Types: Electric

  ┌─────────────────────────────────────────────────────────────────────────────┐
  │   Strong Against: Water, Flying                                             │
  │   Weak Against: Ground, Grass, Dragon                                       │
  │   Resistant To: Electric, Flying, Steel                                     │
  │   Immune To: None                                                           │
  │   Vulnerable To: Ground                                                     │
  └─────────────────────────────────────────────────────────────────────────────┘
```

## 🎯 Tips for Players

- **Type names are case-insensitive**: "Pikachu", "pikachu", or "PIKACHU" all work
- **Use Pokemon names or numbers**: Try "25" instead of "pikachu"
- **Explore different Pokemon**: Each has unique type combinations
- **Study the effectiveness**: Learn which types counter others
- **Use the help menu**: Press "3" for examples and tips

## 🏗️ For Developers

### Build and Test
```bash
# Build the solution
dotnet build

# Run all tests
dotnet test

# Run specific test project
dotnet test tests/PokemonTypeClash.Core.Tests
```

### Create Self-Contained Executables
```bash
# Build single-file executable (recommended for distribution)
dotnet publish src/PokemonTypeClash.Console -c Release -o ./my-app --self-contained -r win-x64 -p:PublishSingleFile=true -p:PublishTrimmed=true
```

### Project Structure
```
src/
├── PokemonTypeClash.Core/          # Domain models and interfaces
├── PokemonTypeClash.Application/   # Business logic and services
├── PokemonTypeClash.Infrastructure/# API clients and data access
└── PokemonTypeClash.Console/       # User interface

tests/
├── PokemonTypeClash.Core.Tests/
├── PokemonTypeClash.Application.Tests/
├── PokemonTypeClash.Infrastructure.Tests/
├── PokemonTypeClash.Console.Tests/
└── PokemonTypeClash.Performance.Tests/
```

## 🐛 Troubleshooting

### Common Issues

**"Command not found: dotnet"**
- Install .NET 9.0 SDK from [Microsoft](https://dotnet.microsoft.com/download)
- **Or** download pre-built executable from [Releases](https://github.com/ryan-hebert/PokemonTypeClash/releases)

**"Permission denied" on run.sh**
- Run: `chmod +x run.sh`

**"No such file or directory" on Windows**
- Use `run.bat` instead of `run.sh`

**"Permission denied" on downloaded executable**
- The pre-built executables have proper permissions set
- If you still get permission errors, run: `chmod +x PokemonTypeClash.Console`

**API errors or timeouts**
- Check your internet connection
- The app will retry automatically

**Build errors**
- Make sure you have .NET 9.0 SDK installed
- Try: `dotnet clean && dotnet build`

## 📚 Learn More

- [Pokemon Type Chart](https://pokemondb.net/type)
- [PokéAPI Documentation](https://pokeapi.co/docs/v2)
- [.NET Documentation](https://docs.microsoft.com/dotnet/)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Submit a pull request

## 📄 License

This project is open source and available under the [MIT License](LICENSE).

---

**Ready to become a Pokemon master? Start analyzing types now! ⚡** 
