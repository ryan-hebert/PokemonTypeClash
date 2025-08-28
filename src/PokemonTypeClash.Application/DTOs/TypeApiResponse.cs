using System.Text.Json.Serialization;

namespace PokemonTypeClash.Application.DTOs;

/// <summary>
/// DTO for Type API response from PokéAPI
/// </summary>
public class TypeApiResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("damage_relations")]
    public DamageRelations DamageRelations { get; set; } = new();
}

/// <summary>
/// DTO for damage relations in Type API response
/// </summary>
public class DamageRelations
{
    [JsonPropertyName("double_damage_to")]
    public List<TypeReference> DoubleDamageTo { get; set; } = new();
    
    [JsonPropertyName("half_damage_to")]
    public List<TypeReference> HalfDamageTo { get; set; } = new();
    
    [JsonPropertyName("no_damage_to")]
    public List<TypeReference> NoDamageTo { get; set; } = new();
    
    [JsonPropertyName("double_damage_from")]
    public List<TypeReference> DoubleDamageFrom { get; set; } = new();
    
    [JsonPropertyName("half_damage_from")]
    public List<TypeReference> HalfDamageFrom { get; set; } = new();
    
    [JsonPropertyName("no_damage_from")]
    public List<TypeReference> NoDamageFrom { get; set; } = new();
}
