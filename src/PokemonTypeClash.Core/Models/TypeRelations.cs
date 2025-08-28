namespace PokemonTypeClash.Core.Models;

/// <summary>
/// Represents the effectiveness relationships for a Pokemon type
/// </summary>
public class TypeRelations
{
    /// <summary>
    /// Types that this type does double damage to
    /// </summary>
    public List<PokemonType> DoubleDamageTo { get; set; } = new();
    
    /// <summary>
    /// Types that this type does half damage to
    /// </summary>
    public List<PokemonType> HalfDamageTo { get; set; } = new();
    
    /// <summary>
    /// Types that this type does no damage to
    /// </summary>
    public List<PokemonType> NoDamageTo { get; set; } = new();
    
    /// <summary>
    /// Types that do double damage to this type
    /// </summary>
    public List<PokemonType> DoubleDamageFrom { get; set; } = new();
    
    /// <summary>
    /// Types that do half damage to this type
    /// </summary>
    public List<PokemonType> HalfDamageFrom { get; set; } = new();
    
    /// <summary>
    /// Types that do no damage to this type
    /// </summary>
    public List<PokemonType> NoDamageFrom { get; set; } = new();
}
