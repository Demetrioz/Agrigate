using Agrigate.Domain.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Agrigate.Domain.Extensions;

public static class DomainExtensions
{
    public static IServiceCollection AddAgrigateDb(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration[""];

        if (string.IsNullOrEmpty(connectionString))
            throw new ApplicationException("Database ConnectionString must be set in configuration");
        
        services.AddDbContext<AgrigateDbContext>(options => options.UseNpgsql(connectionString));
        
        return services;;
    }

    public static void ApplyAgrigateMigrations(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AgrigateDbContext>();
        db.Database.Migrate();
    }
}