using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Agrigate.Core.Configuration;
using Agrigate.Core.Services.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

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

                    options.MapInboundClaims = false;
                    options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
                    options.TokenValidationParameters.RoleClaimType = Constants.Authentication.ClaimTypes.Roles;
                    options.SaveTokens = true;

                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.Events = new OpenIdConnectEvents
                    {
                        OnTokenValidated = AddRolesToClaims
                    };
                });
        
        services.ConfigureCookieOidc(
            CookieAuthenticationDefaults.AuthenticationScheme,
            Constants.Authentication.Policies.Oidc
        );
        
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

        services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.Authority = settings.Authority;
                options.Audience = settings.Audience;
                options.RequireHttpsMetadata = settings.RequireHttpsMetadata;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudience = settings.Audience,

                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                };
            });
        
        return services;
    }

    /// <summary>
    /// Additional cookie configuration for auto-refreshing the token when working with OIDC 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="cookieScheme"></param>
    /// <param name="oidcScheme"></param>
    /// <returns></returns>
    private static IServiceCollection ConfigureCookieOidc(
        this IServiceCollection services,
        string cookieScheme,
        string oidcScheme
    )
    {
        services.AddSingleton<CookieOidcRefresher>();
        services.AddOptions<CookieAuthenticationOptions>(cookieScheme)
            .Configure<CookieOidcRefresher>((cookieOptions, refresher) =>
            {
                cookieOptions.Events.OnValidatePrincipal =
                    context => refresher.ValidateOrRefreshCookieAsync(context, oidcScheme);
            });

        services.AddOptions<OpenIdConnectOptions>(oidcScheme)
            .Configure(oidcOptions =>
            {
                // Request and store a refresh token
                oidcOptions.Scope.Add(OpenIdConnectScope.OfflineAccess);
                oidcOptions.SaveTokens = true;
            });
        
        return services;
    }

    /// <summary>
    /// Ensures that any roles claims included with the JWT are added to the ClaimsIdentity
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private static Task AddRolesToClaims(TokenValidatedContext context)
    {
        if (context?.Principal?.Identity is not ClaimsIdentity claimsIdentity)
            return Task.CompletedTask;
        
        var token = context?.TokenEndpointResponse?.AccessToken;
        if (token == null)
            return Task.CompletedTask;

        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(token))
            return Task.CompletedTask;
        
        var jwt = handler.ReadJwtToken(token);
        var roles = jwt.Claims.Where(c => c.Type == Constants.Authentication.ClaimTypes.Roles).ToList();
        foreach (var role in roles)
            claimsIdentity.AddClaim(new Claim(Constants.Authentication.ClaimTypes.Roles, role.Value));

        return Task.CompletedTask;
    }
}