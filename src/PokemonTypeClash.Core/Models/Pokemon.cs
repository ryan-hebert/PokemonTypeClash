namespace PokemonTypeClash.Core.Models;

/// <summary>
/// Represents a Pokemon with its basic information and types
/// </summary>
public class Pokemon
{
    /// <summary>
    /// The unique identifier for the Pokemon
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// The name of the Pokemon
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// The types of the Pokemon (1-2 types)
    /// </summary>
    public List<PokemonType> Types { get; set; } = new();
    
    /// <summary>
    /// The height of the Pokemon in decimeters
    /// </summary>
    public int Height { get; set; }
    
    /// <summary>
    /// The weight of the Pokemon in hectograms
    /// </summary>
    public int Weight { get; set; }
}
