using Agrigate.Domain.Entities.Common;
using Agrigate.Domain.Entities.Operations;
using Agrigate.Domain.Entities.Production;
using Microsoft.EntityFrameworkCore;

namespace Agrigate.Domain.Contexts;

public class AgrigateContext : DbContext
{
    /// <summary>
    /// Parameterless constructor for creation of migrations
    /// </summary>
    public AgrigateContext()
    {
    }
    
    public AgrigateContext(DbContextOptions<AgrigateContext> options) : base(options)
    {
    }

    public DbSet<Address> Addresses { get; set; }
    
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
    
    public DbSet<Crop> Crops { get; set; }
    public DbSet<Lot> Lots { get; set; }
    public DbSet<Batch> Batches { get; set; }
}