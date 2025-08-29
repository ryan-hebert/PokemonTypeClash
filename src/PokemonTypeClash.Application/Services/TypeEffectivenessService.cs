using Microsoft.Extensions.Logging;
using PokemonTypeClash.Core.Enums;
using System.Diagnostics.CodeAnalysis;
using PokemonTypeClash.Core.Interfaces;
using PokemonTypeClash.Core.Models;

namespace PokemonTypeClash.Application.Services;

/// <summary>
/// Service for calculating type effectiveness and analyzing Pokemon type relationships
/// </summary>
public class TypeEffectivenessService : ITypeEffectivenessService
{
    private readonly ILogger<TypeEffectivenessService> _logger;
    private readonly ITypeDataService _typeDataService;

    public TypeEffectivenessService(ILogger<TypeEffectivenessService> logger, ITypeDataService typeDataService)
    {
        _logger = logger;
        _typeDataService = typeDataService;
    }

    /// <summary>
    /// Analyzes the type effectiveness for a given Pokemon
    /// </summary>
    /// <param name="pokemon">The Pokemon to analyze</param>
    /// <returns>The analysis result</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
    public async Task<TypeAnalysisResult> AnalyzeTypeEffectivenessAsync(Pokemon pokemon)
    {
        _logger.LogInformation("Analyzing type effectiveness for Pokemon: {PokemonName}", pokemon.Name);
        
        var result = new TypeAnalysisResult
        {
            Pokemon = pokemon,
            AnalysisType = AnalysisType.Both
        };

        // Get all Pokemon types for comparison
        var allTypes = await GetAllPokemonTypesAsync();
        
        // Analyze offensive capabilities (what this Pokemon is strong/weak against)
        AnalyzeOffensiveCapabilities(pokemon, allTypes, result);
        
        // Analyze defensive capabilities (what this Pokemon is resistant/vulnerable to)
        AnalyzeDefensiveCapabilities(pokemon, allTypes, result);
        
        _logger.LogInformation("Analysis complete for {PokemonName}. Strong against: {StrongCount}, Weak against: {WeakCount}, Resistant to: {ResistantCount}, Vulnerable to: {VulnerableCount}", 
            pokemon.Name, result.StrongAgainst.Count, result.WeakAgainst.Count, result.ResistantTo.Count, result.VulnerableTo.Count);
        
        return result;
    }

    /// <summary>
    /// Calculates the effectiveness of one type against another
    /// </summary>
    /// <param name="attackingType">The attacking type</param>
    /// <param name="defendingType">The defending type</param>
    /// <returns>The effectiveness multiplier</returns>
    public TypeEffectiveness CalculateEffectiveness(PokemonType attackingType, PokemonType defendingType)
    {
        var effectiveness = TypeEffectiveness.Normal;
        
        // Check if attacking type is super effective against defending type
        if (attackingType.Relations.DoubleDamageTo.Any(t => t.Name.Equals(defendingType.Name, StringComparison.OrdinalIgnoreCase)))
        {
            effectiveness = TypeEffectiveness.SuperEffective;
        }
        // Check if attacking type is not very effective against defending type
        else if (attackingType.Relations.HalfDamageTo.Any(t => t.Name.Equals(defendingType.Name, StringComparison.OrdinalIgnoreCase)))
        {
            effectiveness = TypeEffectiveness.NotVeryEffective;
        }
        // Check if attacking type has no effect against defending type
        else if (attackingType.Relations.NoDamageTo.Any(t => t.Name.Equals(defendingType.Name, StringComparison.OrdinalIgnoreCase)))
        {
            effectiveness = TypeEffectiveness.NoEffect;
        }
        
        return effectiveness;
    }

    /// <summary>
    /// Analyzes the offensive capabilities of a Pokemon
    /// </summary>
    /// <param name="pokemon">The Pokemon to analyze</param>
    /// <param name="allTypes">All available Pokemon types</param>
    /// <param name="result">The analysis result to populate</param>
    private void AnalyzeOffensiveCapabilities(Pokemon pokemon, List<PokemonType> allTypes, TypeAnalysisResult result)
    {
        foreach (var defendingType in allTypes)
        {
            var totalEffectiveness = CalculateMultiTypeOffensiveEffectiveness(pokemon.Types, defendingType);
            
            if (totalEffectiveness > 1.0)
            {
                result.StrongAgainst.Add(defendingType);
            }
            else if (totalEffectiveness < 1.0 && totalEffectiveness > 0)
            {
                result.WeakAgainst.Add(defendingType);
            }
            else if (totalEffectiveness == 0)
            {
                result.NoEffectAgainst.Add(defendingType);
            }
        }
    }

    /// <summary>
    /// Analyzes the defensive capabilities of a Pokemon
    /// </summary>
    /// <param name="pokemon">The Pokemon to analyze</param>
    /// <param name="allTypes">All available Pokemon types</param>
    /// <param name="result">The analysis result to populate</param>
    private void AnalyzeDefensiveCapabilities(Pokemon pokemon, List<PokemonType> allTypes, TypeAnalysisResult result)
    {
        foreach (var attackingType in allTypes)
        {
            var totalEffectiveness = CalculateMultiTypeDefensiveEffectiveness(pokemon.Types, attackingType);
            
            if (totalEffectiveness > 1.0)
            {
                result.VulnerableTo.Add(attackingType);
            }
            else if (totalEffectiveness < 1.0 && totalEffectiveness > 0)
            {
                result.ResistantTo.Add(attackingType);
            }
            else if (totalEffectiveness == 0)
            {
                result.ImmuneTo.Add(attackingType);
            }
        }
    }

    /// <summary>
    /// Calculates the offensive effectiveness of multiple types against a defending type
    /// </summary>
    /// <param name="attackingTypes">The attacking Pokemon's types</param>
    /// <param name="defendingType">The defending type</param>
    /// <returns>The effectiveness multiplier</returns>
    private double CalculateMultiTypeOffensiveEffectiveness(List<PokemonType> attackingTypes, PokemonType defendingType)
    {
        if (!attackingTypes.Any())
            return 1.0;
        
        double totalEffectiveness = 1.0;
        
        foreach (var attackingType in attackingTypes)
        {
            var effectiveness = CalculateEffectiveness(attackingType, defendingType);
            totalEffectiveness *= GetEffectivenessMultiplier(effectiveness);
        }
        
        return totalEffectiveness;
    }

    /// <summary>
    /// Calculates the defensive effectiveness of multiple types against an attacking type
    /// </summary>
    /// <param name="defendingTypes">The defending Pokemon's types</param>
    /// <param name="attackingType">The attacking type</param>
    /// <returns>The effectiveness multiplier</returns>
    private double CalculateMultiTypeDefensiveEffectiveness(List<PokemonType> defendingTypes, PokemonType attackingType)
    {
        if (!defendingTypes.Any())
            return 1.0;
        
        double totalEffectiveness = 1.0;
        
        foreach (var defendingType in defendingTypes)
        {
            var effectiveness = CalculateEffectiveness(attackingType, defendingType);
            totalEffectiveness *= GetEffectivenessMultiplier(effectiveness);
        }
        
        return totalEffectiveness;
    }

    /// <summary>
    /// Converts a TypeEffectiveness enum to a numerical multiplier
    /// </summary>
    /// <param name="effectiveness">The effectiveness enum value</param>
    /// <returns>The numerical multiplier</returns>
    private static double GetEffectivenessMultiplier(TypeEffectiveness effectiveness)
    {
        return effectiveness switch
        {
            TypeEffectiveness.NoEffect => 0.0,
            TypeEffectiveness.NotVeryEffective => 0.5,
            TypeEffectiveness.Normal => 1.0,
            TypeEffectiveness.SuperEffective => 2.0,
            _ => 1.0
        };
    }

    /// <summary>
    /// Gets all available Pokemon types for analysis
    /// </summary>
    /// <returns>List of all Pokemon types</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
    private async Task<List<PokemonType>> GetAllPokemonTypesAsync()
    {
        return await _typeDataService.GetAllTypesAsync();
    }
}
