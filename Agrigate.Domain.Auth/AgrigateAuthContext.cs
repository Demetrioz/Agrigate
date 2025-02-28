using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Agrigate.Domain.Auth;

public class AgrigateAuthContext: IdentityDbContext<AgrigateUser>
{
    /// <summary>
    /// Parameterless constructor for migrations
    /// </summary>
    public AgrigateAuthContext()
    {
    }
    
    /// <summary>
    /// Use an empty string for creating new database migrations
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseNpgsql("");
    }
    
    public AgrigateAuthContext(DbContextOptions<AgrigateAuthContext> options) : base(options)
    {
    }
}