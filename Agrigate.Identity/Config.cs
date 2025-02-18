using Duende.IdentityServer.Models;

namespace Agrigate.Identity;

public static class Config
{
    /// <summary>
    /// The API that powers Agrigate
    /// </summary>
    private const string AgrigateApi = "Agrigate.Api";
    
    /// <summary>
    /// Agrigate's user-facing web application
    /// </summary>
    private const string AgrigateWeb = "Agrigate.Web";

    /// <summary>
    /// Console app for testing machine to machine authentication
    /// </summary>
    private const string AgrigateConsole = "Agrigate.Console";
    
    public static IEnumerable<IdentityResource> IdentityResources =>
    [
        new IdentityResources.OpenId()
    ];

    public static IEnumerable<ApiScope> ApiScopes =>
    [
        new ApiScope(name: AgrigateApi, displayName: "Agrigate API")
    ];

    public static IEnumerable<Client> Clients =>
    [
        // Machine to Machine Client
        new Client
        {
            ClientId = AgrigateConsole,
            AllowedGrantTypes = { GrantType.ClientCredentials },
            ClientSecrets = { new Secret("secret".Sha256()) },
            AllowedScopes = { AgrigateApi }
        }
    ];
}