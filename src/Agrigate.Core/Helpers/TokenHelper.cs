using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;
using Agrigate.Core.Models.Authentication;

namespace Agrigate.Core.Helpers;

/// <summary>
/// Helper class for dealing with authentication tokens
/// </summary>
public static class TokenHelper
{
    /// <summary>
    /// Retrieves KeyCloak roles from a JWT
    /// </summary>
    /// <param name="token"></param>
    /// <param name="assemblyName"></param>
    /// <returns></returns>
    public static List<Claim> GetRolesFromKeyCloakToken(string token, string assemblyName)
    {
        var roles = new List<Claim>();
        
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(token))
            return roles;

        var jwt = handler.ReadJwtToken(token);
        var realmAccess = jwt.Claims.FirstOrDefault(claim => claim.Type == "realm_access")?.Value;
        var resourceAccess = jwt.Claims.FirstOrDefault(claim => claim.Type == "resource_access")?.Value;

        if (!string.IsNullOrWhiteSpace(realmAccess))
        {
            var typedRealmRoles = JsonSerializer.Deserialize<KeyCloakAccess>(realmAccess);
            roles.AddRange(
                (typedRealmRoles?.Roles ?? [])
                    .Select(role => new Claim(Constants.Authentication.ClaimTypes.Roles, role)));
        }

        if (string.IsNullOrWhiteSpace(resourceAccess)) 
            return roles;

        var resources = JsonNode.Parse(resourceAccess);
        if (resources?[assemblyName] == null) 
            return roles;
            
        var typedResourceRoles = resources[assemblyName].Deserialize<KeyCloakAccess>();
        roles.AddRange(
            (typedResourceRoles?.Roles ?? [])
                .Select(role => new Claim(Constants.Authentication.ClaimTypes.Roles, role)));

        return roles;
    }
}