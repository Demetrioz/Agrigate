namespace Agrigate.Core.Configuration;

/// <summary>
/// Configuration settings related to authentication
/// </summary>
public class AuthenticationConfiguration
{
    /// <summary>
    /// The issuer of a JWT
    /// </summary>
    public string Issuer { get; init; } = string.Empty;
    
    /// <summary>
    /// The audience for a JWT
    /// </summary>
    public string Audience { get; init; } = string.Empty;
    
    /// <summary>
    /// A secret key used to sign the JWT
    /// </summary>
    public string SecretKey { get; init; } = string.Empty;
    
    /// <summary>
    /// The number of minutes a JWT should be valid for
    /// </summary>
    public int TokenDurationMinutes { get; init; }
}