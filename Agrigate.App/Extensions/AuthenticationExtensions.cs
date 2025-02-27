using Agrigate.App.Components.Account;
using Agrigate.Domain.Auth;
using Agrigate.Domain.Auth.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Agrigate.App.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddAgrigateAuthentication(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddCascadingAuthenticationState();
        services.AddScoped<IdentityUserAccessor>();
        services.AddScoped<IdentityRedirectManager>();
        services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
        
        services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();

        services.AddAgrigateAuthDb(configuration);
        
        services.AddIdentityCore<AgrigateUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<AgrigateAuthContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();
        
        return services;
    }

    public static WebApplication UseAgrigateAuthentication(this WebApplication app)
    {
        app.Services.ApplyAgrigateAuthMigrations();
        
        // Add additional endpoints required by the Identity /Account Razor components.
        app.MapAdditionalIdentityEndpoints();
        
        return app;
    }
}