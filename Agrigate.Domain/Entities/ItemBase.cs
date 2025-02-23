using System.ComponentModel.DataAnnotations.Schema;
using Agrigate.Domain.Entities.Operations;

namespace Agrigate.Domain.Entities;

/// <summary>
/// Common properties for all item types
/// </summary>
public class ItemBase : EntityBase
{
    public long ItemVariantId { get; set; }
    [ForeignKey(nameof(ItemVariantId))]
    public ItemVariant? ItemVariant { get; set; }
    
    public DateTime? PurchaseDate { get; set; }
    public DateTime? ConsumedDate { get; set; }
    public DateTime? DisposedDate { get; set; }
    public DateTime? DonatedDate { get; set; }

    /// <summary>
    /// Records of when an item was moved. Acts as a history location
    /// </summary>
    [NotMapped]
    public ICollection<ItemTransfer> ItemTransfers { get; set; } = [];
}