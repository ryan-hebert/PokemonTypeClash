# PokemonTypeClash 

A fun console game that analyzes Pokémon type effectiveness! Enter any Pokémon name and see their strengths, weaknesses, and battle strategies with authentic Pokemon-themed styling.

## Quick Start - Play in 3 Steps!

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/PokemonTypeClash.git
cd PokemonTypeClash
```

### 2. Run the Game
```bash
./run.sh
```

### 3. Start Playing!
- Use arrow keys or number keys to navigate
- Choose "Analyze Pokemon Type Effectiveness"
- Enter any Pokemon name (like "pikachu", "charizard", "mewtwo")
- View detailed battle analysis!

## What You'll See

The game provides comprehensive Pokemon battle analysis including:

- **⚔️ Offensive Capabilities**: What types your Pokemon is strong against
- **🛡️ Defensive Capabilities**: What types your Pokemon is resistant to
- **⚠️ Weaknesses**: What types to watch out for
- **🎨 Pokemon-Themed UI**: Authentic colors and styling

## Example Gameplay

```
Enter Pokemon name or ID: pikachu

🐶 PIKACHU
⚡ Types: Electric

┌─────────────────────────────────────────────────────────────────────────────┐
│                           TYPE EFFECTIVENESS TABLE                          │
├─────────────────────────────────────────────────────────────────────────────┤
│  OFFENSIVE CAPABILITIES                                                     │
├─────────────────────────────────────────────────────────────────────────────┤
│  ⚔️  Strong Against: Water, Flying                                          │
│  ⚠️  Weak Against:   Electric, Grass                                        │
│  ❌ No Effect:      Ground                                                  │
├─────────────────────────────────────────────────────────────────────────────┤
│   DEFENSIVE CAPABILITIES                                                    │
├─────────────────────────────────────────────────────────────────────────────┤
│  🛡️  Resistant To:  Flying, Steel, Electric                                │
│  ⚠️  Vulnerable To: Ground                                                  │
└─────────────────────────────────────────────────────────────────────────────┘
```

##  Requirements

- **.NET 9.0 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Git** - [Download here](https://git-scm.com/downloads)

## Alternative Ways to Run

### Option 1: Direct .NET Commands
```bash
dotnet run --project src/PokemonTypeClash.Console
```

### Option 2: Build Then Run
```bash
dotnet build
dotnet run --project src/PokemonTypeClash.Console
```

### Option 3: Use the Launcher Script
```bash
chmod +x run.sh
./run.sh
```

## Game Features

- **Real Pokemon Data**: Uses the official PokéAPI
- **All Pokemon Supported**: Any Pokemon name or ID works
- **Dual-Type Support**: Handles Pokemon with multiple types
- **Interactive Menu**: Arrow key navigation with visual feedback
- **Pokemon-Themed UI**: Authentic colors and styling
- **Network Compatible**: Works on any network configuration

## How to Play

1. **Launch the game** using `./run.sh`
2. **Navigate the menu** with arrow keys or number keys
3. **Choose "Analyze Pokemon Type Effectiveness"**
4. **Enter a Pokemon name** (examples: "pikachu", "charizard", "mewtwo", "25", "6")
5. **View the analysis** and learn battle strategies!
6. **Try another Pokemon** or explore other menu options

## Menu Options

- **⚡ Analyze Pokemon Type Effectiveness** - Main game feature
- **📊 View All Pokemon Types** - See all 18 Pokemon types
- **❓ Help & Examples** - Learn how to play
- **🚪 Exit** - Close the game

## Troubleshooting

### "Command not found: dotnet"
- Install .NET 9.0 SDK from [Microsoft's website](https://dotnet.microsoft.com/download/dotnet/9.0)

### "Permission denied" on run.sh
```bash
chmod +x run.sh
```

### Network issues
- The game automatically handles network compatibility
- Works on corporate networks, VPNs, and firewalls

### Pokemon not found
- Try the Pokemon's ID number instead of name
- Check spelling (names are case-insensitive)
- Examples: "pikachu" (ID: 25), "charizard" (ID: 6)

## Tips for Players

- **Use Pokemon IDs**: If a name doesn't work, try the ID number
- **Explore Dual Types**: Try Pokemon like "charizard" (Fire/Flying) or "venusaur" (Grass/Poison)
- **Learn Type Matchups**: Use the "View All Pokemon Types" option to study
- **Experiment**: Try different Pokemon to understand type relationships

## For Developers

### Running Tests
```bash
dotnet test
```

### Project Structure
```
PokemonTypeClash/
├── src/                    # Source code
│   ├── PokemonTypeClash.Core/           # Domain models
│   ├── PokemonTypeClash.Infrastructure/ # API integration
│   ├── PokemonTypeClash.Application/    # Business logic
│   └── PokemonTypeClash.Console/        # Game UI
├── tests/                  # Test projects
├── run.sh                  # Game launcher
└── README.md              # This file
```


## Acknowledgments

- **PokéAPI** for providing Pokemon data
- **The Pokemon Company** for the Pokemon franchise
- **.NET community** for tooling

---

**Ready to become a Pokemon master? Clone the repo and start analyzing!** 
