# Developer Guide - PokemonTypeClash

## Overview

This guide provides comprehensive information for developers contributing to the PokemonTypeClash project. It covers development setup, architecture patterns, coding standards, and contribution guidelines.

## Table of Contents

1. [Development Environment Setup](#development-environment-setup)
2. [Project Architecture](#project-architecture)
3. [Coding Standards](#coding-standards)
4. [Testing Guidelines](#testing-guidelines)
5. [Adding New Features](#adding-new-features)
6. [Debugging and Troubleshooting](#debugging-and-troubleshooting)
7. [Performance Considerations](#performance-considerations)
8. [Deployment](#deployment)

## Development Environment Setup

### Prerequisites

- **.NET 9.0 SDK** (minimum version 9.0.108)
- **Git** for version control
- **IDE**: Visual Studio 2022, VS Code, or JetBrains Rider
- **Operating System**: Windows, macOS, or Linux

### Installation Steps

1. **Install .NET 9.0 SDK**
   ```bash
   # macOS (using Homebrew)
   brew install dotnet
   
   # Ubuntu/Debian
   wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
   sudo dpkg -i packages-microsoft-prod.deb
   sudo apt-get update
   sudo apt-get install -y dotnet-sdk-9.0
   
   # Windows
   # Download from https://dotnet.microsoft.com/download/dotnet/9.0
   ```

2. **Clone the Repository**
   ```bash
   git clone https://github.com/yourusername/PokemonTypeClash.git
   cd PokemonTypeClash
   ```

3. **Verify Installation**
   ```bash
   dotnet --version  # Should show 9.0.x
   dotnet build      # Should build successfully
   ```

### IDE Configuration

#### Visual Studio Code
Install the following extensions:
- C# Dev Kit
- .NET Extension Pack
- GitLens
- Test Explorer UI

#### Visual Studio 2022
Install the following workloads:
- .NET desktop development
- ASP.NET and web development

## Project Architecture

### Clean Architecture Overview

The project follows Clean Architecture principles with four distinct layers:

```
┌─────────────────────────────────────────────────────────────┐
│                    Presentation Layer                       │
│                 PokemonTypeClash.Console                    │
├─────────────────────────────────────────────────────────────┤
│                    Application Layer                        │
│              PokemonTypeClash.Application                   │
├─────────────────────────────────────────────────────────────┤
│                    Domain Layer                             │
│                 PokemonTypeClash.Core                       │
├─────────────────────────────────────────────────────────────┤
│                  Infrastructure Layer                       │
│              PokemonTypeClash.Infrastructure               │
└─────────────────────────────────────────────────────────────┘
```

### Layer Responsibilities

#### Core Layer (Domain)
- **Purpose**: Contains business logic and domain models
- **Contains**: Entities, interfaces, enums, exceptions
- **Dependencies**: None (pure domain logic)

#### Infrastructure Layer
- **Purpose**: External data access and API integration
- **Contains**: API services, HTTP clients, data mappers
- **Dependencies**: Core layer interfaces

#### Application Layer
- **Purpose**: Business logic orchestration and use cases
- **Contains**: Business services, DTOs, mappers
- **Dependencies**: Core layer interfaces

#### Console Layer (Presentation)
- **Purpose**: User interface and application entry point
- **Contains**: Console UI, commands, Program.cs
- **Dependencies**: All other layers

### Dependency Flow

```
Console → Application → Core ← Infrastructure
```

**Key Principle**: Dependencies point inward toward the Core layer.

## Coding Standards

### C# Coding Conventions

#### Naming Conventions
- **Classes**: PascalCase (e.g., `Pokemon`, `TypeEffectivenessService`)
- **Methods**: PascalCase (e.g., `GetPokemonAsync`, `CalculateEffectiveness`)
- **Properties**: PascalCase (e.g., `Name`, `Types`)
- **Private fields**: camelCase with underscore prefix (e.g., `_httpClient`)
- **Constants**: PascalCase (e.g., `MaxRetries`, `DefaultTimeout`)

#### File Organization
- One public class per file
- File name matches class name
- Group related classes in folders

#### Code Style
```csharp
// Good
public class PokemonService
{
    private readonly IHttpClient _httpClient;
    
    public PokemonService(IHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }
    
    public async Task<Pokemon> GetPokemonAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Pokemon name cannot be null or empty", nameof(name));
            
        // Implementation
    }
}

// Avoid
public class PokemonService {
    private IHttpClient _httpClient;
    public PokemonService(IHttpClient httpClient) { _httpClient = httpClient; }
    public async Task<Pokemon> GetPokemonAsync(string name) { /* implementation */ }
}
```

### SOLID Principles

#### Single Responsibility Principle (SRP)
Each class should have one reason to change.

```csharp
// Good - Single responsibility
public class PokemonApiService : IPokemonApiService
{
    public async Task<Pokemon> GetPokemonAsync(string name) { /* implementation */ }
}

// Avoid - Multiple responsibilities
public class PokemonService
{
    public async Task<Pokemon> GetPokemonAsync(string name) { /* implementation */ }
    public void SaveToDatabase(Pokemon pokemon) { /* implementation */ }
    public void SendEmail(string message) { /* implementation */ }
}
```

#### Open/Closed Principle (OCP)
Open for extension, closed for modification.

```csharp
// Good - Extensible through interfaces
public interface ITypeEffectivenessService
{
    TypeEffectiveness CalculateEffectiveness(PokemonType attacking, PokemonType defending);
}

public class TypeEffectivenessService : ITypeEffectivenessService
{
    public virtual TypeEffectiveness CalculateEffectiveness(PokemonType attacking, PokemonType defending)
    {
        // Base implementation
    }
}

// Extension through inheritance
public class AdvancedTypeEffectivenessService : TypeEffectivenessService
{
    public override TypeEffectiveness CalculateEffectiveness(PokemonType attacking, PokemonType defending)
    {
        // Enhanced implementation
    }
}
```

#### Dependency Inversion Principle (DIP)
Depend on abstractions, not concretions.

```csharp
// Good - Depends on interface
public class PokemonAnalyzer
{
    private readonly IPokemonApiService _pokemonService;
    
    public PokemonAnalyzer(IPokemonApiService pokemonService)
    {
        _pokemonService = pokemonService;
    }
}

// Avoid - Depends on concrete class
public class PokemonAnalyzer
{
    private readonly PokemonApiService _pokemonService;
    
    public PokemonAnalyzer(PokemonApiService pokemonService)
    {
        _pokemonService = pokemonService;
    }
}
```

### Async/Await Patterns

#### Proper Async Usage
```csharp
// Good
public async Task<Pokemon> GetPokemonAsync(string name)
{
    var response = await _httpClient.GetAsync($"pokemon/{name}");
    var content = await response.Content.ReadAsStringAsync();
    return JsonSerializer.Deserialize<Pokemon>(content);
}

// Avoid - Blocking calls
public Pokemon GetPokemon(string name)
{
    var response = _httpClient.GetAsync($"pokemon/{name}").Result;
    var content = response.Content.ReadAsStringAsync().Result;
    return JsonSerializer.Deserialize<Pokemon>(content);
}
```

#### Exception Handling
```csharp
// Good - Proper async exception handling
public async Task<Pokemon> GetPokemonAsync(string name)
{
    try
    {
        var response = await _httpClient.GetAsync($"pokemon/{name}");
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Pokemon>(content);
    }
    catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
    {
        throw new PokemonNotFoundException(name);
    }
    catch (HttpRequestException ex)
    {
        throw new PokemonApiException($"Failed to retrieve Pokemon: {ex.Message}", (int?)ex.StatusCode ?? 500);
    }
}
```

## Testing Guidelines

### Test Structure

#### Test Project Organization
```
tests/
├── PokemonTypeClash.Core.Tests/
│   ├── Models/
│   ├── Enums/
│   └── Exceptions/
├── PokemonTypeClash.Application.Tests/
│   └── Services/
├── PokemonTypeClash.Infrastructure.Tests/
│   ├── Services/
│   ├── Mappers/
│   └── Integration/
└── PokemonTypeClash.Console.Tests/
    └── UI/
```

#### Test Naming Convention
```csharp
[Fact]
public void CalculateEffectiveness_WhenElectricAttacksWater_ReturnsSuperEffective()
{
    // Arrange
    var electricType = new PokemonType { Name = "electric" };
    var waterType = new PokemonType { Name = "water" };
    
    // Act
    var result = _service.CalculateEffectiveness(electricType, waterType);
    
    // Assert
    result.Should().Be(TypeEffectiveness.SuperEffective);
}
```

### Test Categories

#### Unit Tests
- **Purpose**: Test individual components in isolation
- **Dependencies**: Mocked using Moq
- **Execution**: Fast, no external dependencies

```csharp
[Fact]
public void GetPokemonAsync_WithValidName_ReturnsPokemon()
{
    // Arrange
    var expectedPokemon = new Pokemon { Name = "pikachu" };
    _mockApiService.Setup(x => x.GetPokemonAsync("pikachu"))
                  .ReturnsAsync(expectedPokemon);
    
    // Act
    var result = await _service.GetPokemonAsync("pikachu");
    
    // Assert
    result.Should().BeEquivalentTo(expectedPokemon);
}
```

#### Integration Tests
- **Purpose**: Test component interactions
- **Dependencies**: Real external services (API calls)
- **Execution**: Slower, requires network access

```csharp
[Fact, Category("Integration")]
public async Task GetPokemonAsync_WithRealApi_ReturnsValidPokemon()
{
    // Arrange
    var service = new PokemonApiService(_httpClient);
    
    // Act
    var result = await service.GetPokemonAsync("pikachu");
    
    // Assert
    result.Should().NotBeNull();
    result.Name.Should().Be("pikachu");
    result.Types.Should().ContainSingle();
}
```

### Mocking Guidelines

#### Using Moq
```csharp
// Setup mock behavior
_mockService.Setup(x => x.GetDataAsync(It.IsAny<string>()))
           .ReturnsAsync(new TestData());

// Verify interactions
_mockService.Verify(x => x.GetDataAsync("test"), Times.Once);

// Setup exceptions
_mockService.Setup(x => x.GetDataAsync("invalid"))
           .ThrowsAsync(new NotFoundException());
```

### Test Data Builders

#### Fluent Test Data Creation
```csharp
public class PokemonBuilder
{
    private string _name = "pikachu";
    private List<PokemonType> _types = new();
    
    public PokemonBuilder WithName(string name)
    {
        _name = name;
        return this;
    }
    
    public PokemonBuilder WithType(string typeName)
    {
        _types.Add(new PokemonType { Name = typeName });
        return this;
    }
    
    public Pokemon Build()
    {
        return new Pokemon
        {
            Name = _name,
            Types = _types
        };
    }
}

// Usage
var pokemon = new PokemonBuilder()
    .WithName("charizard")
    .WithType("fire")
    .WithType("flying")
    .Build();
```

## Adding New Features

### Feature Development Workflow

1. **Create Feature Branch**
   ```bash
   git checkout -b feature/new-feature-name
   ```

2. **Implement Feature**
   - Follow Clean Architecture principles
   - Add comprehensive tests
   - Update documentation

3. **Test Locally**
   ```bash
   dotnet build
   dotnet test
   dotnet run --project src/PokemonTypeClash.Console
   ```

4. **Create Pull Request**
   - Include detailed description
   - Reference related issues
   - Request code review

### Adding New Services

#### Step 1: Define Interface (Core Layer)
```csharp
// src/PokemonTypeClash.Core/Interfaces/INewService.cs
public interface INewService
{
    Task<Result> DoSomethingAsync(string input);
}
```

#### Step 2: Implement Service (Infrastructure/Application Layer)
```csharp
// src/PokemonTypeClash.Application/Services/NewService.cs
public class NewService : INewService
{
    public async Task<Result> DoSomethingAsync(string input)
    {
        // Implementation
    }
}
```

#### Step 3: Register in DI Container
```csharp
// src/PokemonTypeClash.Console/Configuration/ServiceCollectionExtensions.cs
public static IServiceCollection AddApplicationServices(this IServiceCollection services)
{
    services.AddScoped<INewService, NewService>();
    return services;
}
```

#### Step 4: Add Tests
```csharp
// tests/PokemonTypeClash.Application.Tests/Services/NewServiceTests.cs
public class NewServiceTests
{
    [Fact]
    public async Task DoSomethingAsync_WithValidInput_ReturnsSuccess()
    {
        // Test implementation
    }
}
```

### Adding New Models

#### Step 1: Create Model (Core Layer)
```csharp
// src/PokemonTypeClash.Core/Models/NewModel.cs
public class NewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

#### Step 2: Add Validation
```csharp
public class NewModel
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
```

#### Step 3: Add Tests
```csharp
[Fact]
public void NewModel_WithValidData_IsValid()
{
    var model = new NewModel
    {
        Id = 1,
        Name = "Test"
    };
    
    model.Name.Should().Be("Test");
    model.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
}
```

## Debugging and Troubleshooting

### Common Issues

#### Build Errors
```bash
# Clean and rebuild
dotnet clean
dotnet build

# Restore packages
dotnet restore

# Check .NET version
dotnet --version
```

#### Test Failures
```bash
# Run specific test
dotnet test --filter "FullyQualifiedName~TestName"

# Run with verbose output
dotnet test --verbosity normal

# Run integration tests only
dotnet test --filter "Category=Integration"
```

#### Runtime Errors
```bash
# Run with detailed logging
dotnet run --project src/PokemonTypeClash.Console --verbosity normal

# Check configuration
cat appsettings.json
```

### Debugging Techniques

#### Using Debugger
```csharp
// Add breakpoints in IDE
// Or use Debugger.Break() in code
if (Debugger.IsAttached)
{
    Debugger.Break();
}
```

#### Logging
```csharp
public class PokemonService
{
    private readonly ILogger<PokemonService> _logger;
    
    public async Task<Pokemon> GetPokemonAsync(string name)
    {
        _logger.LogInformation("Retrieving Pokemon: {Name}", name);
        
        try
        {
            var result = await _apiService.GetPokemonAsync(name);
            _logger.LogInformation("Successfully retrieved Pokemon: {Name}", name);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve Pokemon: {Name}", name);
            throw;
        }
    }
}
```

## Performance Considerations

### Memory Management
- Use `using` statements for disposable objects
- Avoid large object allocations in hot paths
- Implement proper caching strategies

### Async Performance
- Use `ConfigureAwait(false)` for library code
- Avoid blocking calls in async methods
- Use cancellation tokens for long-running operations

### API Optimization
- Implement request caching
- Use connection pooling
- Implement retry policies with exponential backoff

## Deployment

### Local Development
```bash
# Run application
dotnet run --project src/PokemonTypeClash.Console

# Run with specific configuration
dotnet run --project src/PokemonTypeClash.Console --environment Development
```

### Production Build
```bash
# Publish for production
dotnet publish src/PokemonTypeClash.Console -c Release -o ./publish

# Run published application
./publish/PokemonTypeClash.Console
```

### Docker Deployment
```bash
# Build Docker image
docker build -t pokemontypeclash .

# Run container
docker run -it pokemontypeclash
```

## Contributing Guidelines

### Code Review Checklist
- [ ] Code follows project conventions
- [ ] All tests pass
- [ ] New features have tests
- [ ] Documentation is updated
- [ ] No breaking changes (unless documented)

### Commit Message Format
```
type(scope): description

[optional body]

[optional footer]
```

**Types:**
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `style`: Code style changes
- `refactor`: Code refactoring
- `test`: Test changes
- `chore`: Build/tooling changes

**Examples:**
```
feat(api): add new Pokemon type analysis endpoint
fix(ui): resolve console display issue with long names
docs(readme): update installation instructions
```

### Pull Request Process
1. Fork the repository
2. Create feature branch
3. Make changes following guidelines
4. Add/update tests
5. Update documentation
6. Submit pull request
7. Address review feedback
8. Merge after approval

## Resources

### Documentation
- [.NET 9 Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [C# Programming Guide](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

### Tools
- [xUnit Testing Framework](https://xunit.net/)
- [Moq Mocking Library](https://github.com/moq/moq4)
- [FluentAssertions](https://fluentassertions.com/)

### External APIs
- [PokéAPI Documentation](https://pokeapi.co/docs/v2)
