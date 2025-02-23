using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Agrigate.Domain.Enums.Operations;

namespace Agrigate.Domain.Entities.Operations;

/// <summary>
/// Represents a master list of items used in the operation of a farm or garden 
/// </summary>
[Table(nameof(Item))]
public class Item : EntityBase
{
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string Description { get; set; } = string.Empty;
    
    public ItemType Type { get; set; }
    
    public ICollection<ItemVariant> Variants { get; set; } = [];
}