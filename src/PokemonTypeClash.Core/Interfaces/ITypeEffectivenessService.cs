using PokemonTypeClash.Core.Models;
using PokemonTypeClash.Core.Enums;

namespace PokemonTypeClash.Core.Interfaces;

/// <summary>
/// Service for calculating type effectiveness and analyzing Pokemon type relationships
/// </summary>
public interface ITypeEffectivenessService
{
    /// <summary>
    /// Analyzes the type effectiveness for a given Pokemon
    /// </summary>
    /// <param name="pokemon">The Pokemon to analyze</param>
    /// <returns>The analysis result</returns>
    Task<TypeAnalysisResult> AnalyzeTypeEffectivenessAsync(Pokemon pokemon);
    
    /// <summary>
    /// Calculates the effectiveness of one type against another
    /// </summary>
    /// <param name="attackingType">The attacking type</param>
    /// <param name="defendingType">The defending type</param>
    /// <returns>The effectiveness level</returns>
    TypeEffectiveness CalculateEffectiveness(PokemonType attackingType, PokemonType defendingType);
}


