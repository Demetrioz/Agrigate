using Agrigate.Core.Configuration;

namespace Agrigate.Api.Configuration;

/// <summary>
/// Configuration settings for the API
/// </summary>
public class ApiOptions
{
    /// <summary>
    /// Service configuration for the API
    /// </summary>
    public ServiceOptions ApiService { get; set; } = new();

    /// <summary>
    /// Service configuration for the EventService
    /// </summary>
    public ServiceOptions EventService { get; set; } = new();
}