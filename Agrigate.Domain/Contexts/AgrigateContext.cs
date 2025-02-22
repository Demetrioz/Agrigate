using Agrigate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Agrigate.Domain.Contexts;

public class AgrigateContext : DbContext
{
    public AgrigateContext(DbContextOptions<AgrigateContext> options) : base(options)
    {
    }
    
    // System Tables
    public DbSet<Log> Logs { get; set; }
}