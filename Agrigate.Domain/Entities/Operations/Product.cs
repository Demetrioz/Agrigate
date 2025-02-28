using System.ComponentModel.DataAnnotations.Schema;
using Agrigate.Domain.Entities.Production;

namespace Agrigate.Domain.Entities.Operations;

/// <summary>
/// Represents a product that has been created on the farm
/// </summary>
[Table(nameof(Product))]
public class Product : ItemBase
{
    public long BatchId { get; set; }
    [ForeignKey(nameof(BatchId))]
    public Batch? Batch { get; set; }
}