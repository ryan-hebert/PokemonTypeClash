using System.Diagnostics.CodeAnalysis;

namespace PokemonTypeClash.Infrastructure.Http;

/// <summary>
/// Interface for HTTP client wrapper for making requests to the PokéAPI
/// </summary>
public interface IPokeApiHttpClient
{
    /// <summary>
    /// Makes a GET request to the PokéAPI and deserializes the response
    /// </summary>
    /// <typeparam name="T">The type to deserialize the response to</typeparam>
    /// <param name="endpoint">The API endpoint to call</param>
    /// <returns>The deserialized response</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
    Task<T> GetAsync<T>(string endpoint);
}
