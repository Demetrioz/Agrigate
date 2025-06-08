namespace Agrigate.Web.Services.JwtService;

/// <summary>
/// Service for interacting with JWTs
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Attempts to retrieve a cached token, otherwise generates a new token
    /// </summary>
    /// <param name="email">The email address of a user</param>
    /// <returns></returns>
    Task<string> GetOrCreateToken(string email);
}