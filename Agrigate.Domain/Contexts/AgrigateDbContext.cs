using Agrigate.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;

namespace Agrigate.Domain.Contexts;

public class AgrigateDbContext : DbContext
{
    /// <summary>
    /// Parameterless constructor for migrations
    /// </summary>
    public AgrigateDbContext()
    {
    }
    
    public AgrigateDbContext(DbContextOptions<AgrigateDbContext> options) : base(options)
    {
    }
    
    // System Tables
    public DbSet<Setting> Settings { get; set; }
}