using System.ComponentModel.DataAnnotations.Schema;

namespace Agrigate.Domain.Entities.Operations;

/// <summary>
/// Tracks the use of items for production
/// </summary>
[Table(nameof(ItemTransaction))]
public class ItemTransaction : EntityBase
{
    public DateTime TransactionDate { get; set; }
    
    public ICollection<ItemTransactionInput> Inputs { get; set; } = [];
    public ICollection<ItemTransactionOutput> Outputs { get; set; } = [];
}