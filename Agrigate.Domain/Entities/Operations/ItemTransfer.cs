using System.ComponentModel.DataAnnotations.Schema;
using Agrigate.Domain.Enums.Operations;

namespace Agrigate.Domain.Entities.Operations;

/// <summary>
/// Represents the movement of items from one location to another
/// </summary>
[Table(nameof(ItemTransfer))]
public class ItemTransfer : EntityBase
{
    public ItemType Type { get; set; }
   
    /// <summary>
    /// The ItemId could refer to a Consumable, Product,
    /// or Equipment based on the value of Type
    /// </summary>
    public long ItemId { get; set; }
   
    [NotMapped]
    public Consumable? Consumable { get; set; }
   
    [NotMapped]
    public Equipment? Equipment { get; set; }
   
    [NotMapped]
    public Product? Product { get; set; }
   
    public DateTime TransferDate { get; set; }
   
    /// <summary>
    /// The original location of the item
    /// </summary>
    public long SourceLocationId { get; set; }
    [ForeignKey(nameof(SourceLocationId))]
    public Location? SourceLocation { get; set; }
   
    /// <summary>
    /// The new location of the item
    /// </summary>
    public long DestinationLocationId { get; set; }
    [ForeignKey(nameof(DestinationLocationId))]
    public Location? DestinationLocation { get; set; }
}