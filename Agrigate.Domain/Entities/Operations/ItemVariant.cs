using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agrigate.Domain.Entities.Operations;

/// <summary>
/// Represents a variant of an item
/// </summary>
[Table(nameof(ItemVariant))]
public class ItemVariant : EntityBase
{
    public long ItemId { get; set; }
    [ForeignKey(nameof(ItemId))]
    public Item? Item { get; set; }

    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// A type describing the type of unit, such as head, packet, plant, etc
    /// </summary>
    [MaxLength(255)]
    public string UnitType { get; set; } = string.Empty;
    
    /// <summary>
    /// The SI unit of measure, such as oz, g, lb, etc
    /// </summary>
    [MaxLength(255)]
    public string? Unit { get; set; }
    
    /// <summary>
    /// The size of the unit, such as 1, 0.4
    /// </summary>
    public double Size { get; set; }
    
    public bool RemindOnLowQuantity { get; set; }
    public int LowQuantityReminderCount { get; set; }
}