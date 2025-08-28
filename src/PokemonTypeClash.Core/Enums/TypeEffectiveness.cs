namespace PokemonTypeClash.Core.Enums;

/// <summary>
/// Represents the effectiveness of a type against another type
/// </summary>
public enum TypeEffectiveness
{
    /// <summary>
    /// No effect - 0x damage
    /// </summary>
    NoEffect = 0,
    
    /// <summary>
    /// Not very effective - 0.5x damage
    /// </summary>
    NotVeryEffective = 1,
    
    /// <summary>
    /// Normal effectiveness - 1x damage
    /// </summary>
    Normal = 2,
    
    /// <summary>
    /// Super effective - 2x damage
    /// </summary>
    SuperEffective = 3
}
