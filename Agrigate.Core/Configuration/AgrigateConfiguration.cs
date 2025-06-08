namespace Agrigate.Core.Configuration;

/// <summary>
/// Configuration settings shared across Agrigate applications
/// </summary>
public class AgrigateConfiguration
{
    /// <summary>
    /// The Database connection string
    /// </summary>
    public string? DbConnectionString { get; set; }
    
    /// <summary>
    /// The default email address for logging in to Agrigate
    /// </summary>
    public string? DefaultEmail { get; set; }
    
    /// <summary>
    /// The default password for logging in to Agrigate 
    /// </summary>
    public string? DefaultPassword { get; set; }
    
    /// <summary>
    /// The URL to use when making requests to the Agrigate API
    /// </summary>
    public string? ApiUrl { get; set; }
}