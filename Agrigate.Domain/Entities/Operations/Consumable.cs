using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agrigate.Domain.Entities.Operations;

/// <summary>
/// Represents consumable items used in the operation of a farm
/// </summary>
[Table(nameof(Consumable))]
public class Consumable : ItemBase
{
    public long? SupplierId { get; set; }
    [ForeignKey(nameof(SupplierId))]
    public Supplier? Supplier { get; set; }
    
    [MaxLength(255)]
    public string? Barcode { get; set; }
    
    [MaxLength(128)]
    public string? Sku { get; set; }
    
    [MaxLength(128)]
    public string? Lot { get; set; }
    
    [MaxLength(128)]
    public string? Batch { get; set; }
}