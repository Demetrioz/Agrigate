using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;
using Agrigate.Core.Configuration;
using Agrigate.Core.Helpers;
using Agrigate.Core.Models.Authentication;
using Agrigate.Core.Services.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

using JwtContext = Microsoft.AspNetCore.Authentication.JwtBearer.TokenValidatedContext;
using OpenIdContext = Microsoft.AspNetCore.Authentication.OpenIdConnect.TokenValidatedContext;

namespace Agrigate.Core.Extensions;

/// <summary>
/// Extensions to help simplify setting up authentication
/// </summary>
public static class AuthenticationExtensions
{
    /// <summary>
    /// Configures Authentication for a web application using OIDC & Cookies
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddOidcAuthentication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var settings = new AuthenticationConfiguration();
        configuration.Bind(Constants.Authentication.Configuration, settings);

        var sourceProject = CoreHelper.GetSourceNamespace();
        
        services.AddAuthentication(Constants.Authentication.Policies.Oidc)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(
                Constants.Authentication.Policies.Oidc,
                options =>
                {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.Authority = settings.Authority;
                    options.ClientId = settings.ClientId;
                    options.ClientSecret = settings.ClientSecret;
                    options.RequireHttpsMetadata = settings.RequireHttpsMetadata;
                    options.ResponseType = OpenIdConnectResponseType.Code;

                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.MapInboundClaims = false;
                    options.SaveTokens = true;
                    
                    options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
                    options.TokenValidationParameters.RoleClaimType = Constants.Authentication.ClaimTypes.Roles;

                    options.Scope.Clear();
                    options.Scope.Add("email");
                    options.Scope.Add("roles");
                    options.Scope.Add("openid");
                    options.Scope.Add(OpenIdConnectScope.OfflineAccess);
                    
                    options.Events = new OpenIdConnectEvents
                    {
                        OnTokenValidated = (context) => AddRolesToClaims(context, sourceProject)
                    };
                });
        
        services.AddSingleton<CookieOidcRefresher>();
        services.AddOptions<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme)
            .Configure<CookieOidcRefresher>((cookieOptions, refresher) =>
            {
                cookieOptions.Events.OnValidatePrincipal =
                    context => refresher.ValidateOrRefreshCookieAsync(context, Constants.Authentication.Policies.Oidc);
        });
        
        return services;
    }

    /// <summary>
    /// Configures authentication for a web api using JWTs
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var settings = new AuthenticationConfiguration();
        configuration.Bind(Constants.Authentication.Configuration, settings);

        var sourceProject = CoreHelper.GetSourceNamespace();
        
        services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.Authority = settings.Authority;
                options.Audience = settings.Audience;
                
                options.RequireHttpsMetadata = settings.RequireHttpsMetadata;
                options.MetadataAddress = settings.MetadataAddress;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = settings.Authority,
                    ValidAudiences = settings.AllowedAudiences,
                    
                    NameClaimType = JwtRegisteredClaimNames.Name,
                    RoleClaimType = Constants.Authentication.ClaimTypes.Roles,

                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = (context) => AddRolesToClaims(context, sourceProject)
                };
            });
        
        return services;
    }

    /// <summary>
    /// Adds endpoints to handle authentication routes for a web application
    /// See https://github.com/dotnet/blazor-samples/tree/main/8.0/BlazorWebAppOidcServer
    /// </summary>
    /// <param name="endpoints"></param>
    /// <returns></returns>
    public static void MapAuthenticationRoutes(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGroup(Constants.Authentication.Routes.Auth)
            .MapLoginLogout();
    }

    /// <summary>
    /// Ensures that any roles claims included with the JWT are added to the ClaimsIdentity
    /// </summary>
    /// <param name="context"></param>
    /// <param name="assemblyName"></param>
    /// <returns></returns>
    private static Task AddRolesToClaims(OpenIdContext context, string assemblyName)
    {
        if (context.Principal?.Identity is not ClaimsIdentity claimsIdentity)
            return Task.CompletedTask;
        
        var token = context.TokenEndpointResponse?.AccessToken;
        if (token == null)
            return Task.CompletedTask;

        var claims = TokenHelper.GetRolesFromKeyCloakToken(token, assemblyName);
        if (claims.Count > 0)
            claimsIdentity.AddClaims(claims);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Ensures that any roles claims included with the KeyCloak JWT are added to the ClaimsIdentity
    /// </summary>
    /// <param name="context"></param>
    /// <param name="assemblyName"></param>
    /// <returns></returns>
    private static Task AddRolesToClaims(JwtContext context, string assemblyName)
    {
        if (context.Principal?.Identity is not ClaimsIdentity claimsIdentity)
            return Task.CompletedTask;

        var realmAccess = claimsIdentity.FindFirst(claim => claim.Type == "realm_access")?.Value;
        var resourceAccess = claimsIdentity.FindFirst(claim => claim.Type == "resource_access")?.Value;

        if (!string.IsNullOrWhiteSpace(realmAccess))
        {
            var typedRealmRoles = JsonSerializer.Deserialize<KeyCloakAccess>(realmAccess);
            foreach (var role in typedRealmRoles?.Roles ?? [])
                claimsIdentity.AddClaim(new Claim(Constants.Authentication.ClaimTypes.Roles, role));
        }

        if (string.IsNullOrWhiteSpace(resourceAccess)) 
            return Task.CompletedTask;

        var resources = JsonNode.Parse(resourceAccess);
        if (resources?[assemblyName] == null) 
            return Task.CompletedTask;
            
        var typedResourceRoles = resources[assemblyName].Deserialize<KeyCloakAccess>();
        foreach (var role in typedResourceRoles?.Roles ?? [])
            claimsIdentity.AddClaim(new Claim(Constants.Authentication.ClaimTypes.Roles, role));

        return Task.CompletedTask;
    }
    
    /// <summary>
    /// Adds endpoints to handle logging in and out of the application.
    /// See https://github.com/dotnet/blazor-samples/tree/main/8.0/BlazorWebAppOidcServer
    /// </summary>
    /// <param name="endpoints"></param>
    /// <returns></returns>
    private static void MapLoginLogout(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("");

        group
            .MapGet(
                Constants.Authentication.Routes.Login,
                (string? returnUrl) => TypedResults.Challenge(GetAuthProperties(returnUrl))
            )
            .AllowAnonymous();

        group
            .MapPost(
                Constants.Authentication.Routes.Logout,
                ([FromForm] string? returnUrl) => 
                    TypedResults.SignOut(
                        GetAuthProperties(returnUrl),
                        [
                            CookieAuthenticationDefaults.AuthenticationScheme, 
                            Constants.Authentication.Policies.Oidc
                        ]
                    )
            );
    }
    
    /// <summary>
    /// Helper to retrieve authentication properties during login & logout
    /// </summary>
    /// <param name="returnUrl"></param>
    /// <returns></returns>
    private static AuthenticationProperties GetAuthProperties(string? returnUrl)
    {
        // TODO: Use HttpContext.Request.PathBase instead.
        const string pathBase = "/";

        // Prevent open redirects.
        if (string.IsNullOrEmpty(returnUrl))
        {
            returnUrl = pathBase;
        }
        else if (!Uri.IsWellFormedUriString(returnUrl, UriKind.Relative))
        {
            returnUrl = new Uri(returnUrl, UriKind.Absolute).PathAndQuery;
        }
        else if (returnUrl[0] != '/')
        {
            returnUrl = $"{pathBase}{returnUrl}";
        }

        return new AuthenticationProperties { RedirectUri = returnUrl };
    }
}