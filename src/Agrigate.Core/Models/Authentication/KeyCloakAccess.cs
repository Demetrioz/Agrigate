using System.Text.Json.Serialization;

namespace Agrigate.Core.Models.Authentication;

/// <summary>
/// Model for accessing roles stored in a KeyCloak access token 
/// </summary>
public class KeyCloakAccess
{
    /// <summary>
    /// A list of roles assigned to the user 
    /// </summary>
    [JsonPropertyName("roles")]
    public List<string> Roles { get; set; } = [];
}