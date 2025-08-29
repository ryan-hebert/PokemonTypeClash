using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using PokemonTypeClash.Core.Exceptions;
using PokemonTypeClash.Infrastructure.Configuration;

namespace PokemonTypeClash.Infrastructure.Http;

/// <summary>
/// HTTP client wrapper for making requests to the PokéAPI
/// </summary>
public class PokeApiHttpClient : IPokeApiHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PokeApiHttpClient> _logger;
    private readonly PokeApiConfiguration _configuration;
    private readonly JsonSerializerOptions _jsonOptions;

    public PokeApiHttpClient(
        HttpClient httpClient,
        ILogger<PokeApiHttpClient> logger,
        PokeApiConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;
        
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    /// <summary>
    /// Makes a GET request to the PokéAPI and deserializes the response
    /// </summary>
    /// <typeparam name="T">The type to deserialize the response to</typeparam>
    /// <param name="endpoint">The API endpoint to call</param>
    /// <returns>The deserialized response</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
    public async Task<T> GetAsync<T>(string endpoint)
    {
        // Use relative URL since BaseAddress is configured on HttpClient
        var relativeUrl = endpoint.TrimStart('/');
        
        _logger.LogInformation("Making request to PokéAPI: {Endpoint}", relativeUrl);
        
        for (int attempt = 1; attempt <= _configuration.MaxRetries; attempt++)
        {
            try
            {
                // Add a small delay between retries to avoid overwhelming the API
                if (attempt > 1)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(500));
                }
                
                var response = await _httpClient.GetAsync(relativeUrl);
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<T>(content, _jsonOptions);
                    
                    if (result == null)
                    {
                        throw new PokemonApiException($"Failed to deserialize response from {relativeUrl}");
                    }
                    
                    _logger.LogInformation("Successfully retrieved data from {Endpoint}", relativeUrl);
                    return result;
                }
                
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new PokemonNotFoundException(endpoint);
                }
                
                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    _logger.LogWarning("Rate limited by PokéAPI, attempt {Attempt} of {MaxRetries}", attempt, _configuration.MaxRetries);
                    
                    if (attempt < _configuration.MaxRetries)
                    {
                        var delay = TimeSpan.FromSeconds(Math.Pow(2, attempt)); // Exponential backoff
                        await Task.Delay(delay);
                        continue;
                    }
                }
                
                throw new PokemonApiException($"HTTP {(int)response.StatusCode}: {response.ReasonPhrase}");
            }
            catch (PokemonNotFoundException)
            {
                throw; // Re-throw immediately, no retry for not found
            }
            catch (Exception ex) when (ex is not PokemonApiException)
            {
                _logger.LogError(ex, "Error making request to {Endpoint}, attempt {Attempt} of {MaxRetries}", relativeUrl, attempt, _configuration.MaxRetries);
                
                if (attempt < _configuration.MaxRetries)
                {
                    var delay = TimeSpan.FromSeconds(Math.Pow(2, attempt)); // Exponential backoff
                    await Task.Delay(delay);
                    continue;
                }
                
                throw new PokemonApiException($"Failed to communicate with PokéAPI after {_configuration.MaxRetries} attempts", ex);
            }
        }
        
        throw new PokemonApiException($"Failed to retrieve data from {relativeUrl} after {_configuration.MaxRetries} attempts");
    }
}
