namespace PokemonTypeClash.Infrastructure.Configuration;

/// <summary>
/// Configuration settings for the PokéAPI
/// </summary>
public class PokeApiConfiguration
{
    /// <summary>
    /// The base URL for the PokéAPI
    /// </summary>
    public string BaseUrl { get; set; } = "https://pokeapi.co/api/v2/";
    
    /// <summary>
    /// Timeout in seconds for API requests
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;
    
    /// <summary>
    /// Maximum number of retry attempts for failed requests
    /// </summary>
    public int MaxRetries { get; set; } = 3;
    
    /// <summary>
    /// Cache duration in minutes for type data
    /// </summary>
    public int CacheDurationMinutes { get; set; } = 60;
}
