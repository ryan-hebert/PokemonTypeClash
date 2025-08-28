namespace PokemonTypeClash.Core.Models;

/// <summary>
/// Represents a Pokemon type with its effectiveness relationships
/// </summary>
public class PokemonType
{
    /// <summary>
    /// The unique identifier for the type
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// The name of the type (e.g., "fire", "water", "grass")
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// The type effectiveness relationships
    /// </summary>
    public TypeRelations Relations { get; set; } = new();
}
