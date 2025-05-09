namespace Agrigate.Api.Configuration;

/// <summary>
/// Configuration settings related to authentication
/// </summary>
public class AuthenticationConfiguration
{
    /// <summary>
    /// The IDP host URL
    /// </summary>
    public string Authority { get; set; } = string.Empty;
    
    /// <summary>
    /// The audience required to make requests
    /// </summary>
    public string Audience { get; set; } = string.Empty;
    
    /// <summary>
    /// Whether to require HTTPS metadata. Should be true in production
    /// </summary>
    public bool RequireHttpsMetadata { get; set; }
}