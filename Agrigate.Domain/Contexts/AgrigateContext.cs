using Agrigate.Domain.Entities.Common;
using Agrigate.Domain.Entities.Crops;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Agrigate.Domain.Contexts;

/// <summary>
/// Database Context for Agrigate
/// </summary>
public class AgrigateContext : IdentityDbContext<AgrigateUser>
{
    public AgrigateContext(DbContextOptions<AgrigateContext> options) : base(options)
    {
    }

    /// <summary>
    /// Parameterless constsructor for migrations
    /// </summary>
    public AgrigateContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseNpgsql("");
    }
    
    public DbSet<CropDetail> CropDetails { get; set; }
}