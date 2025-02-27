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
        app.Services.AddDefaultUser(app.Configuration).GetAwaiter().GetResult();
        
        // Add additional endpoints required by the Identity /Account Razor components.
        app.MapAdditionalIdentityEndpoints();
        
        return app;
    }

    private static async Task AddDefaultUser(this IServiceProvider serviceProvider, IConfiguration configuration)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AgrigateUser>>();

        var defaultUsername = configuration["Authentication:DefaultUser"];
        var defaultPassword = configuration["Authentication:DefaultPassword"];
        
        if (string.IsNullOrEmpty(defaultUsername) || string.IsNullOrEmpty(defaultPassword))
            throw new ApplicationException("Default Username and Password must be set in configuration");
        
        var defaultUser = await userManager.FindByNameAsync(defaultUsername);
        if (defaultUser == null)
        {
            defaultUser = new AgrigateUser { UserName = defaultUsername };
            await userManager.CreateAsync(defaultUser, defaultPassword);
        }
    }
}