using Agrigate.Core;
using Agrigate.Core.Configuration;
using Microsoft.OpenApi.Models;

namespace Agrigate.Api.Extensions;

/// <summary>
/// Extensions for implementing and configuring Swagger
/// </summary>
public static class SwaggerExtensions
{
    /// <summary>
    /// Adds Swagger with authentication configured to allow login via KeyCloak
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void ConfigureSwaggerWithAuth(this IServiceCollection services, IConfiguration configuration)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        
        var settings = new AuthenticationConfiguration();
        configuration.Bind(Constants.Authentication.Configuration, settings);
        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));
            options.AddSecurityDefinition(
                "Keycloak",
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{settings.Authority}/protocol/openid-connect/auth"),
                            Scopes =
                            {
                                { "openid", "openid" },
                                { "profile", "profile" },
                            }
                        }
                    }
                }
            );

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Keycloak",
                            Type = ReferenceType.SecurityScheme
                        },
                        In = ParameterLocation.Header,
                        Name = "Bearer",
                        Scheme = "Bearer",
                    },
                    []
                }
            };
    
            options.AddSecurityRequirement(securityRequirement);
        });
    }
}