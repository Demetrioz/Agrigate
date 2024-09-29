using Agrigate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

    /// <summary>
    /// Parameterless constructor for migrations
    /// </summary>
    public AgrigateContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json", optional: false)
                .Build();

            var connectionString = config.GetConnectionString("AgrigateDb");
            optionsBuilder.UseNpgsql(connectionString);
        }
    }

    public DbSet<Device> Devices { get; set; }
    public DbSet<Telemetry> Telemetry { get; set; }
}