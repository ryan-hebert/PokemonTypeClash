using System.Text.Json.Serialization;

namespace PokemonTypeClash.Application.DTOs;

/// <summary>
/// DTO for Pokemon API response from PokéAPI
/// </summary>
public class PokemonApiResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("height")]
    public int Height { get; set; }
    
    [JsonPropertyName("weight")]
    public int Weight { get; set; }
    
    [JsonPropertyName("types")]
    public List<PokemonTypeSlot> Types { get; set; } = new();
}

/// <summary>
/// DTO for Pokemon type slot in API response
/// </summary>
public class PokemonTypeSlot
{
    [JsonPropertyName("slot")]
    public int Slot { get; set; }
    
    [JsonPropertyName("type")]
    public TypeReference Type { get; set; } = new();
}

/// <summary>
/// DTO for type reference in API response
/// </summary>
public class TypeReference
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
}
