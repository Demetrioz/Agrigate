using Agrigate.Domain.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Agrigate.Domain.Extensions;

public static class DomainExtensions
{
    public static IServiceCollection AddAggrigateDomain(this IServiceCollection services)
    {
        services.AddDbContextFactory<AgrigateContext>(options =>
            options.UseSqlite("Filename=Agrigate.db"));
        
        return services;
    }
}