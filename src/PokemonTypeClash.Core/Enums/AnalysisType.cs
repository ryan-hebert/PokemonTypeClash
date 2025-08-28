namespace PokemonTypeClash.Core.Enums;

/// <summary>
/// Represents the type of analysis to perform on a Pokemon
/// </summary>
public enum AnalysisType
{
    /// <summary>
    /// Analyze what this Pokemon's attacks are effective against
    /// </summary>
    Offensive,
    
    /// <summary>
    /// Analyze what this Pokemon takes damage from
    /// </summary>
    Defensive,
    
    /// <summary>
    /// Complete analysis of both offensive and defensive capabilities
    /// </summary>
    Both
}
