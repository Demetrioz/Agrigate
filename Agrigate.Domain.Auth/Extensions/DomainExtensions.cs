using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Agrigate.Domain.Auth.Extensions;

public static class DomainExtensions
{
    public static IServiceCollection AddAgrigateAuthDb(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["Authentication:ConnectionString"];
        
        if (string.IsNullOrEmpty(connectionString))
            throw new ApplicationException("Authentication ConnectionString must be set in configuration");
        
        services.AddDbContext<AgrigateAuthContext>(options => options.UseNpgsql(connectionString));
        
        return services;
    }

    public static void ApplyAgrigateAuthMigrations(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AgrigateAuthContext>();
        db.Database.Migrate();
    }
}