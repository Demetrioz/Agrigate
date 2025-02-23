using System.ComponentModel.DataAnnotations.Schema;
using Agrigate.Domain.Enums.Operations;

namespace Agrigate.Domain.Entities.Operations;

[Table(nameof(ItemTransactionOutput))]
public class ItemTransactionOutput : EntityBase
{
    public ItemType Type { get; set; }
    
    /// <summary>
    /// The ItemId could refer to a Consumable or Product
    /// based on the value type
    /// </summary>
    public long ItemId { get; set; }
    
    [NotMapped]
    public Consumable? Consumable { get; set; }
    
    [NotMapped]
    public Product? Product { get; set; }
}