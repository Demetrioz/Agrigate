using Agrigate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Agrigate.Domain.Contexts;

/// <summary>
/// Database for the Agrigate platform
/// </summary>
public class AgrigateContext : DbContext
{
    public AgrigateContext(DbContextOptions<AgrigateContext> options)
        : base(options)
    {
    }

    public DbSet<Device> Devices { get; set; }
    public DbSet<Telemetry> Telemetry { get; set; }
}