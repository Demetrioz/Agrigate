using System.ComponentModel.DataAnnotations.Schema;
using Agrigate.Domain.Enums.Operations;

namespace Agrigate.Domain.Entities.Operations;

[Table(nameof(ItemTransactionInput))]
public class ItemTransactionInput : EntityBase
{
    public ItemType Type { get; set; }
    
    /// <summary>
    /// The ItemId could refer to a Consumable or Product
    /// based on the value of Type
    /// </summary>
    public long InputId { get; set; }
    
    [NotMapped]
    public Consumable? Consumable { get; set; }
    
    [NotMapped]
    public Product? Product { get; set; }
}