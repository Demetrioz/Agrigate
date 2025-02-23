using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Agrigate.Domain.Entities.Operations;

namespace Agrigate.Domain.Entities.Production;

/// <summary>
/// Represents the output of a Crop's harvest, associated with a Lot
/// </summary>
[Table(nameof(Batch))]
public class Batch : EntityBase
{
    public long LotId { get; set; }
    [ForeignKey(nameof(LotId))]
    public Lot? Lot { get; set; }

    [MaxLength(128)]
    public string BatchNumber { get; set; } = string.Empty;
    
    public ICollection<Product> Products { get; set; } = [];
}