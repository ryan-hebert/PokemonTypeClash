using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PokemonTypeClash.Console.UI;
using PokemonTypeClash.Core.Enums;
using PokemonTypeClash.Core.Interfaces;
using PokemonTypeClash.Core.Models;

namespace PokemonTypeClash.Console.Tests;

public class ConsoleUITests
{
    private readonly Mock<ITypeEffectivenessService> _typeEffectivenessServiceMock;
    private readonly Mock<IPokemonApiService> _pokemonApiServiceMock;
    private readonly ConsoleUI _consoleUI;

    public ConsoleUITests()
    {
        _typeEffectivenessServiceMock = new Mock<ITypeEffectivenessService>();
        _pokemonApiServiceMock = new Mock<IPokemonApiService>();
        _consoleUI = new ConsoleUI(_typeEffectivenessServiceMock.Object, _pokemonApiServiceMock.Object);
    }

    [Fact]
    public void ShowMainMenu_ShouldDisplayCorrectMenu()
    {
        // Arrange
        var stringWriter = new StringWriter();
        System.Console.SetOut(stringWriter);

        // Act
        _consoleUI.ShowMainMenu();

        // Assert
        var output = stringWriter.ToString();
        output.Should().Contain("PokemonTypeClash");
        output.Should().Contain("⚡ 1. Analyze Pokemon Type Effectiveness");
        output.Should().Contain("📊 2. View All Pokemon Types");
        output.Should().Contain("❓ 3. Help & Examples");
        output.Should().Contain("🚪 4. Exit");

        // Cleanup
        System.Console.SetOut(new StreamWriter(System.Console.OpenStandardOutput()));
    }

    [Fact]
    public void ShowError_ShouldDisplayErrorInRed()
    {
        // Arrange
        var stringWriter = new StringWriter();
        System.Console.SetOut(stringWriter);
        var errorMessage = "Test error message";

        // Act
        _consoleUI.ShowError(errorMessage);

        // Assert
        var output = stringWriter.ToString();
        output.Should().Contain($"❌ Error: {errorMessage}");

        // Cleanup
        System.Console.SetOut(new StreamWriter(System.Console.OpenStandardOutput()));
    }

    [Fact]
    public void ShowSuccess_ShouldDisplaySuccessInGreen()
    {
        // Arrange
        var stringWriter = new StringWriter();
        System.Console.SetOut(stringWriter);
        var successMessage = "Test success message";

        // Act
        _consoleUI.ShowSuccess(successMessage);

        // Assert
        var output = stringWriter.ToString();
        output.Should().Contain($"✅ {successMessage}");

        // Cleanup
        System.Console.SetOut(new StreamWriter(System.Console.OpenStandardOutput()));
    }



    [Fact]
    public void ShowLoading_ShouldDisplayLoadingMessage()
    {
        // Arrange
        var stringWriter = new StringWriter();
        System.Console.SetOut(stringWriter);
        var loadingMessage = "Loading data";

        // Act
        _consoleUI.ShowLoading(loadingMessage);

        // Assert
        var output = stringWriter.ToString();
        output.Should().Contain($"⏳ {loadingMessage}... ");

        // Cleanup
        System.Console.SetOut(new StreamWriter(System.Console.OpenStandardOutput()));
    }

    [Fact]
    public void ClearLoading_ShouldDisplayDoneMessage()
    {
        // Arrange
        var stringWriter = new StringWriter();
        System.Console.SetOut(stringWriter);

        // Act
        _consoleUI.ClearLoading();

        // Assert
        var output = stringWriter.ToString();
        output.Should().Contain("Done! ✨");

        // Cleanup
        System.Console.SetOut(new StreamWriter(System.Console.OpenStandardOutput()));
    }

    [Fact]
    public void DisplayAnalysisResult_WithValidResult_ShouldDisplayCorrectly()
    {
        // Arrange
        var stringWriter = new StringWriter();
        System.Console.SetOut(stringWriter);
        
        var pokemon = new Pokemon
        {
            Id = 25,
            Name = "pikachu",
            Types = new List<PokemonType> { new() { Name = "electric" } }
        };

        var result = new TypeAnalysisResult
        {
            Pokemon = pokemon,
            StrongAgainst = new List<PokemonType> { new() { Name = "water" } },
            WeakAgainst = new List<PokemonType> { new() { Name = "grass" } },
            NoEffectAgainst = new List<PokemonType> { new() { Name = "ground" } },
            ResistantTo = new List<PokemonType> { new() { Name = "flying" } },
            VulnerableTo = new List<PokemonType> { new() { Name = "ground" } },
            ImmuneTo = new List<PokemonType>(),
            AnalysisType = AnalysisType.Both
        };

        // Act
        _consoleUI.DisplayAnalysisResult(result);

        // Assert
        var output = stringWriter.ToString();
        output.Should().Contain("TYPE ANALYSIS");
        output.Should().Contain("⚡ Types: Electric");
        output.Should().Contain("TYPE EFFECTIVENESS TABLE");
        output.Should().Contain("OFFENSIVE CAPABILITIES");
        output.Should().Contain("⚔️  Strong Against:");
        output.Should().Contain("Water");
        output.Should().Contain("⚠️  Weak Against:");
        output.Should().Contain("Grass");
        output.Should().Contain("❌ No Effect:");
        output.Should().Contain("Ground");
        output.Should().Contain("DEFENSIVE CAPABILITIES");
        output.Should().Contain("🛡️  Resistant To:");
        output.Should().Contain("Flying");
        output.Should().Contain("⚠️  Vulnerable To:");
        output.Should().Contain("Ground");

        // Cleanup
        System.Console.SetOut(new StreamWriter(System.Console.OpenStandardOutput()));
    }





    [Theory]
    [InlineData("y", true)]
    [InlineData("Y", true)]
    [InlineData("yes", true)]
    [InlineData("YES", true)]
    [InlineData("n", false)]
    [InlineData("N", false)]
    [InlineData("no", false)]
    [InlineData("NO", false)]
    [InlineData("", false)]
    [InlineData("invalid", false)]
    public void AskToContinue_WithVariousInputs_ShouldReturnCorrectResult(string input, bool expectedResult)
    {
        // Arrange
        var stringReader = new StringReader(input);
        System.Console.SetIn(stringReader);

        // Act
        var result = _consoleUI.AskToContinue();

        // Assert
        result.Should().Be(expectedResult);

        // Cleanup
        System.Console.SetIn(new StreamReader(System.Console.OpenStandardInput()));
    }

    [Fact]
    public void GetUserInput_WithPrompt_ShouldReturnUserInput()
    {
        // Arrange
        var userInput = "test input";
        var stringReader = new StringReader(userInput);
        System.Console.SetIn(stringReader);
        var stringWriter = new StringWriter();
        System.Console.SetOut(stringWriter);
        var prompt = "Enter test";

        // Act
        var result = _consoleUI.GetUserInput(prompt);

        // Assert
        result.Should().Be(userInput);
        var output = stringWriter.ToString();
        output.Should().Contain($"🎯 {prompt}: ");

        // Cleanup
        System.Console.SetIn(new StreamReader(System.Console.OpenStandardInput()));
        System.Console.SetOut(new StreamWriter(System.Console.OpenStandardOutput()));
    }

    [Fact]
    public void GetUserInput_WithNullInput_ShouldReturnEmptyString()
    {
        // Arrange
        var stringReader = new StringReader("");
        System.Console.SetIn(stringReader);
        var stringWriter = new StringWriter();
        System.Console.SetOut(stringWriter);

        // Act
        var result = _consoleUI.GetUserInput("Test");

        // Assert
        result.Should().Be(string.Empty);

        // Cleanup
        System.Console.SetIn(new StreamReader(System.Console.OpenStandardInput()));
        System.Console.SetOut(new StreamWriter(System.Console.OpenStandardOutput()));
    }
}
