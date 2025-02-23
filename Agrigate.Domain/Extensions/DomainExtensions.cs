using Agrigate.Domain.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Agrigate.Domain.Extensions;

public static class DomainExtensions
{
    public static IServiceCollection AddAggrigateDomain(this IServiceCollection services)
    {
        services.AddDbContextFactory<AgrigateContext>(options =>
            options.UseSqlite("Filename=Agrigate.db"));
        
        return services;
    }

    public static void ApplyMigrations(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
        
        logger.Information("Attempting to apply database migrations");
        
        var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AgrigateContext>>();
        using var dbContext = dbFactory.CreateDbContext();
        dbContext.Database.Migrate();
        
        logger.Information("Successfully applied database migrations");
    }
}