namespace PokemonTypeClash.Core.Exceptions;

/// <summary>
/// Exception thrown when there's an error communicating with the PokéAPI
/// </summary>
public class PokemonApiException : Exception
{
    /// <summary>
    /// Initializes a new instance of the PokemonApiException class
    /// </summary>
    /// <param name="message">The error message</param>
    public PokemonApiException(string message) : base(message)
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the PokemonApiException class
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="innerException">The inner exception</param>
    public PokemonApiException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when a Pokemon is not found
/// </summary>
public class PokemonNotFoundException : PokemonApiException
{
    /// <summary>
    /// The name or ID that was searched for
    /// </summary>
    public string SearchedValue { get; }
    
    /// <summary>
    /// Initializes a new instance of the PokemonNotFoundException class
    /// </summary>
    /// <param name="searchedValue">The name or ID that was searched for</param>
    public PokemonNotFoundException(string searchedValue) 
        : base($"Pokemon '{searchedValue}' was not found. Please check the spelling and try again.")
    {
        SearchedValue = searchedValue;
    }
}

/// <summary>
/// Exception thrown when a Pokemon type is not found
/// </summary>
public class PokemonTypeNotFoundException : PokemonApiException
{
    /// <summary>
    /// The name or ID that was searched for
    /// </summary>
    public string SearchedValue { get; }
    
    /// <summary>
    /// Initializes a new instance of the PokemonTypeNotFoundException class
    /// </summary>
    /// <param name="searchedValue">The name or ID that was searched for</param>
    public PokemonTypeNotFoundException(string searchedValue) 
        : base($"Pokemon type '{searchedValue}' was not found. Please check the spelling and try again.")
    {
        SearchedValue = searchedValue;
    }
}
