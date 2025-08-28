using PokemonTypeClash.Core.Enums;

namespace PokemonTypeClash.Core.Models;

/// <summary>
/// Represents the result of a Pokemon type effectiveness analysis
/// </summary>
public class TypeAnalysisResult
{
    /// <summary>
    /// The Pokemon that was analyzed
    /// </summary>
    public Pokemon Pokemon { get; set; } = new();
    
    /// <summary>
    /// Types that this Pokemon's attacks are super effective against
    /// </summary>
    public List<PokemonType> StrongAgainst { get; set; } = new();
    
    /// <summary>
    /// Types that this Pokemon's attacks are not very effective against
    /// </summary>
    public List<PokemonType> WeakAgainst { get; set; } = new();
    
    /// <summary>
    /// Types that this Pokemon's attacks have no effect against
    /// </summary>
    public List<PokemonType> NoEffectAgainst { get; set; } = new();
    
    /// <summary>
    /// Types that this Pokemon is resistant to (takes reduced damage)
    /// </summary>
    public List<PokemonType> ResistantTo { get; set; } = new();
    
    /// <summary>
    /// Types that this Pokemon is vulnerable to (takes super effective damage)
    /// </summary>
    public List<PokemonType> VulnerableTo { get; set; } = new();
    
    /// <summary>
    /// Types that this Pokemon is immune to (takes no damage)
    /// </summary>
    public List<PokemonType> ImmuneTo { get; set; } = new();
    
    /// <summary>
    /// The type of analysis that was performed
    /// </summary>
    public AnalysisType AnalysisType { get; set; }
    
    /// <summary>
    /// Timestamp when the analysis was performed
    /// </summary>
    public DateTime AnalysisTimestamp { get; set; } = DateTime.UtcNow;
}
