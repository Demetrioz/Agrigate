namespace Agrigate.Web.Services.JwtService;

/// <summary>
/// Service for interacting with JWTs
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Generates a JWT for a user with the given email
    /// </summary>
    /// <param name="email">The email address of a user</param>
    /// <returns></returns>
    Task<string> GenerateToken(string email);
}