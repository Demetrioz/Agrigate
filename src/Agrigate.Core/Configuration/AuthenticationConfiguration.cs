namespace Agrigate.Core.Configuration;

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
    
    /// <summary>
    /// The client's id from the IDP
    /// </summary>
    public string ClientId { get; set; } = string.Empty;
    
    /// <summary>
    /// The client's secret from the IDP
    /// </summary>
    public string ClientSecret { get; set; } = string.Empty;
}