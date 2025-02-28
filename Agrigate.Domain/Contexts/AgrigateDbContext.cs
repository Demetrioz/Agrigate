using Agrigate.Domain.Entities.Common;
using Agrigate.Domain.Entities.Operations;
using Agrigate.Domain.Entities.Production;
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

    /// <summary>
    /// Use an empty string for creating new database migrations
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseNpgsql("");
    }
    
    public AgrigateDbContext(DbContextOptions<AgrigateDbContext> options) : base(options)
    {
    }
    
    // System Tables
    public DbSet<Setting> Settings { get; set; }
    
    // Shared Tables
    public DbSet<Address> Addresses { get; set; }
    
    // Operational Tables
    public DbSet<Supplier> Suppliers { get; set; }
    
    public DbSet<Location> Locations { get; set; }
    public DbSet<LocationMetadata> LocationMetaData { get; set; }
    
    public DbSet<Item> Items { get; set; }
    public DbSet<ItemVariant> ItemVariants { get; set; }
    public DbSet<ItemTransaction> ItemTransactions { get; set; }
    public DbSet<ItemTransactionInput> ItemTransactionInputs { get; set; }
    public DbSet<ItemTransactionOutput> ItemTransactionOutputs { get; set; }
    public DbSet<ItemTransfer> ItemTransfers { get; set; }
    public DbSet<Consumable> Consumables { get; set; }
    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<Product> Products { get; set; }
    
    // Production Tables
    public DbSet<Crop> Crops { get; set; }
    public DbSet<Lot> Lots { get; set; }
    public DbSet<Batch> Batches { get; set; }
}