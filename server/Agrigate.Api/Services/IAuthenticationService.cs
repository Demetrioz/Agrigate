namespace Agrigate.Api.Services;

/// <summary>
/// Service for handling authentication-related tasks for the Agrigate API
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Determines whether a provided api key is valid
    /// </summary>
    /// <param name="apiKey">The api key to validate</param>
    /// <returns></returns>
    bool IsValidKey(string apiKey);
}