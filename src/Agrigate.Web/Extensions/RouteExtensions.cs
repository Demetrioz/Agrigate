using Agrigate.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Agrigate.Web.Extensions;

/// <summary>
/// Extensions to assist with setting up various routes
/// </summary>
public static class RouteExtensions
{
    /// <summary>
    /// Adds endpoints to handle logging in and out of the application.
    /// See https://github.com/dotnet/blazor-samples/tree/main/8.0/BlazorWebAppOidcServer
    /// </summary>
    /// <param name="endpoints"></param>
    /// <returns></returns>
    public static IEndpointConventionBuilder MapAuthenticationRoutes(this IEndpointRouteBuilder endpoints)
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
        
        return group;
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