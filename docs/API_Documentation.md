# API Documentation - PokemonTypeClash

## Overview

This document provides comprehensive API documentation for the PokemonTypeClash application, covering all service interfaces, data models, and external API integrations.

## Core Domain Models

### Pokemon
Represents a Pokémon entity with its basic information and type data.

```csharp
public class Pokemon
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<PokemonType> Types { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
}
```

**Properties:**
- `Id`: Unique identifier from PokéAPI
- `Name`: Pokémon name (e.g., "pikachu", "charizard")
- `Types`: List of Pokémon types (1-2 types possible)
- `Height`: Height in decimeters
- `Weight`: Weight in hectograms

### PokemonType
Represents a Pokémon type with its effectiveness relationships.

```csharp
public class PokemonType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public TypeRelations Relations { get; set; }
}
```

**Properties:**
- `Id`: Type identifier from PokéAPI
- `Name`: Type name (e.g., "electric", "fire", "water")
- `Relations`: Type effectiveness relationships

### TypeRelations
Contains all type effectiveness relationships for damage calculations.

```csharp
public class TypeRelations
{
    public List<PokemonType> DoubleDamageTo { get; set; }
    public List<PokemonType> HalfDamageTo { get; set; }
    public List<PokemonType> NoDamageTo { get; set; }
    public List<PokemonType> DoubleDamageFrom { get; set; }
    public List<PokemonType> HalfDamageFrom { get; set; }
    public List<PokemonType> NoDamageFrom { get; set; }
}
```

**Properties:**
- `DoubleDamageTo`: Types this type is super effective against (2x damage)
- `HalfDamageTo`: Types this type is not very effective against (0.5x damage)
- `NoDamageTo`: Types this type has no effect against (0x damage)
- `DoubleDamageFrom`: Types that are super effective against this type
- `HalfDamageFrom`: Types that are not very effective against this type
- `NoDamageFrom`: Types that have no effect against this type

### TypeAnalysisResult
Result of type effectiveness analysis for a Pokémon.

```csharp
public class TypeAnalysisResult
{
    public Pokemon Pokemon { get; set; }
    public List<PokemonType> StrongAgainst { get; set; }
    public List<PokemonType> WeakAgainst { get; set; }
    public List<PokemonType> ResistantTo { get; set; }
    public List<PokemonType> VulnerableTo { get; set; }
    public AnalysisType AnalysisType { get; set; }
}
```

**Properties:**
- `Pokemon`: The analyzed Pokémon
- `StrongAgainst`: Types the Pokémon is strong against
- `WeakAgainst`: Types the Pokémon is weak against
- `ResistantTo`: Types the Pokémon resists
- `VulnerableTo`: Types the Pokémon is vulnerable to
- `AnalysisType`: Type of analysis performed (Offensive/Defensive/Both)

## Enums

### TypeEffectiveness
Represents the effectiveness level of a type matchup.

```csharp
public enum TypeEffectiveness
{
    NoEffect = 0,        // 0x damage
    NotVeryEffective,    // 0.5x damage
    Normal,              // 1x damage
    SuperEffective       // 2x damage
}
```

### AnalysisType
Specifies the type of analysis to perform.

```csharp
public enum AnalysisType
{
    Offensive,  // What this Pokemon's attacks are effective against
    Defensive,  // What this Pokemon takes damage from
    Both        // Complete analysis
}
```

## Service Interfaces

### IPokemonApiService
Service for retrieving Pokémon data from the PokéAPI.

```csharp
public interface IPokemonApiService
{
    Task<Pokemon> GetPokemonAsync(string nameOrId);
    Task<PokemonType> GetTypeAsync(string nameOrId);
}
```

**Methods:**
- `GetPokemonAsync(string nameOrId)`: Retrieves Pokémon data by name or ID
- `GetTypeAsync(string nameOrId)`: Retrieves type data by name or ID

**Parameters:**
- `nameOrId`: Pokémon name (case-insensitive) or numeric ID

**Returns:**
- `Task<Pokemon>`: Pokémon entity with complete type information
- `Task<PokemonType>`: Type entity with effectiveness relationships

**Exceptions:**
- `PokemonApiException`: API communication errors
- `PokemonNotFoundException`: Pokémon not found (404)

### ITypeEffectivenessService
Service for calculating type effectiveness and performing analysis.

```csharp
public interface ITypeEffectivenessService
{
    Task<TypeAnalysisResult> AnalyzeTypeEffectivenessAsync(Pokemon pokemon, AnalysisType analysisType = AnalysisType.Both);
    TypeEffectiveness CalculateEffectiveness(PokemonType attackingType, PokemonType defendingType);
}
```

**Methods:**
- `AnalyzeTypeEffectivenessAsync(Pokemon pokemon, AnalysisType analysisType)`: Performs complete type analysis
- `CalculateEffectiveness(PokemonType attackingType, PokemonType defendingType)`: Calculates effectiveness between two types

**Parameters:**
- `pokemon`: Pokémon to analyze
- `analysisType`: Type of analysis (default: Both)
- `attackingType`: Type performing the attack
- `defendingType`: Type being attacked

**Returns:**
- `Task<TypeAnalysisResult>`: Complete analysis result
- `TypeEffectiveness`: Effectiveness level (0x, 0.5x, 1x, 2x)

### IConsoleUI
Service for console user interface interactions.

```csharp
public interface IConsoleUI
{
    void ShowMainMenu();
    string GetUserInput(string prompt);
    void DisplayAnalysisResult(TypeAnalysisResult result);
    void ShowError(string message);
    void ShowLoadingMessage(string message);
    void ClearScreen();
}
```

**Methods:**
- `ShowMainMenu()`: Displays the main application menu
- `GetUserInput(string prompt)`: Gets user input with prompt
- `DisplayAnalysisResult(TypeAnalysisResult result)`: Displays formatted analysis results
- `ShowError(string message)`: Displays error messages
- `ShowLoadingMessage(string message)`: Shows loading/processing messages
- `ClearScreen()`: Clears the console screen

## External API Integration

### PokéAPI Endpoints

#### Pokemon Data
```
GET https://pokeapi.co/api/v2/pokemon/{name-or-id}
```

**Response Example:**
```json
{
  "id": 25,
  "name": "pikachu",
  "height": 4,
  "weight": 60,
  "types": [
    {
      "slot": 1,
      "type": {
        "name": "electric",
        "url": "https://pokeapi.co/api/v2/type/13/"
      }
    }
  ]
}
```

#### Type Data
```
GET https://pokeapi.co/api/v2/type/{type-name-or-id}
```

**Response Example:**
```json
{
  "id": 13,
  "name": "electric",
  "damage_relations": {
    "double_damage_to": [
      {"name": "water", "url": "https://pokeapi.co/api/v2/type/11/"},
      {"name": "flying", "url": "https://pokeapi.co/api/v2/type/3/"}
    ],
    "half_damage_to": [
      {"name": "electric", "url": "https://pokeapi.co/api/v2/type/13/"},
      {"name": "grass", "url": "https://pokeapi.co/api/v2/type/12/"},
      {"name": "dragon", "url": "https://pokeapi.co/api/v2/type/16/"}
    ],
    "no_damage_to": [
      {"name": "ground", "url": "https://pokeapi.co/api/v2/type/5/"}
    ],
    "double_damage_from": [
      {"name": "ground", "url": "https://pokeapi.co/api/v2/type/5/"}
    ],
    "half_damage_from": [
      {"name": "flying", "url": "https://pokeapi.co/api/v2/type/3/"},
      {"name": "steel", "url": "https://pokeapi.co/api/v2/type/9/"},
      {"name": "electric", "url": "https://pokeapi.co/api/v2/type/13/"}
    ],
    "no_damage_from": []
  }
}
```

### Error Handling

#### HTTP Status Codes
- `200 OK`: Successful response
- `404 Not Found`: Pokémon or type not found
- `429 Too Many Requests`: Rate limit exceeded
- `500 Internal Server Error`: Server error
- `503 Service Unavailable`: Service temporarily unavailable

#### Exception Types
```csharp
public class PokemonApiException : Exception
{
    public int StatusCode { get; }
    public PokemonApiException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}

public class PokemonNotFoundException : Exception
{
    public string PokemonName { get; }
    public PokemonNotFoundException(string pokemonName) : base($"Pokemon '{pokemonName}' not found")
    {
        PokemonName = pokemonName;
    }
}
```

## Configuration

### appsettings.json
```json
{
  "PokeApi": {
    "BaseUrl": "https://pokeapi.co/api/v2/",
    "TimeoutSeconds": 30,
    "MaxRetries": 3,
    "CacheDurationMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "System": "Warning"
    }
  }
}
```

**Configuration Properties:**
- `BaseUrl`: PokéAPI base URL
- `TimeoutSeconds`: HTTP request timeout
- `MaxRetries`: Maximum retry attempts for failed requests
- `CacheDurationMinutes`: Type data cache duration

## Usage Examples

### Basic Pokémon Analysis
```csharp
// Get Pokémon data
var pokemon = await pokemonApiService.GetPokemonAsync("pikachu");

// Analyze type effectiveness
var analysis = await typeEffectivenessService.AnalyzeTypeEffectivenessAsync(pokemon);

// Display results
consoleUI.DisplayAnalysisResult(analysis);
```

### Type Effectiveness Calculation
```csharp
// Get type data
var electricType = await pokemonApiService.GetTypeAsync("electric");
var waterType = await pokemonApiService.GetTypeAsync("water");

// Calculate effectiveness
var effectiveness = typeEffectivenessService.CalculateEffectiveness(electricType, waterType);
// Returns: TypeEffectiveness.SuperEffective (2x damage)
```

### Error Handling
```csharp
try
{
    var pokemon = await pokemonApiService.GetPokemonAsync("invalid-pokemon");
}
catch (PokemonNotFoundException ex)
{
    consoleUI.ShowError($"Pokemon not found: {ex.PokemonName}");
}
catch (PokemonApiException ex)
{
    consoleUI.ShowError($"API Error: {ex.Message}");
}
```

## Performance Considerations

### Caching
- Type data is cached for 60 minutes to reduce API calls
- Pokémon data is not cached (frequently changing)
- Cache is in-memory and per-instance

### Rate Limiting
- PokéAPI has rate limits (100 requests per minute)
- Application implements exponential backoff retry logic
- Requests are spaced appropriately to respect limits

### Memory Usage
- Application typically uses < 50MB during operation
- Type data cache can grow to ~2MB with all types loaded
- Pokémon data is not persisted between requests

## Testing

### Unit Tests
All services have comprehensive unit tests covering:
- Normal operation scenarios
- Error conditions
- Edge cases
- Type effectiveness calculations

### Integration Tests
Integration tests verify:
- Real API communication
- Error handling with actual API responses
- End-to-end functionality

### Test Categories
- `[Fact]`: Unit tests (no external dependencies)
- `[Fact, Category("Integration")]`: Integration tests (external API calls)

Run tests with:
```bash
# Unit tests only
dotnet test --filter "Category!=Integration"

# All tests including integration
dotnet test
```
