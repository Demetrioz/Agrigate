using Duende.IdentityServer;
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
        new IdentityResources.OpenId(),
        new IdentityResources.Profile()
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
        },
        // Interactive .Net Core Web App
        new Client
        {
            ClientId = AgrigateWeb,
            AllowedGrantTypes = GrantTypes.Code,
            ClientSecrets = { new Secret("secret".Sha256()) },
            RedirectUris = { "http://localhost:5003/signin-oidc" },
            PostLogoutRedirectUris = { "http://localhost:5003/signout-callback-oidc" },
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
            }
        }
    ];
}