using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agrigate.Domain.Entities.Production;

/// <summary>
/// Identifier generated at the time a crop is planted / created 
/// </summary>
[Table(nameof(Lot))]
public class Lot : EntityBase
{
    public long CropId { get; set; }
    [ForeignKey(nameof(CropId))]
    public Crop? Crop { get; set; }
    
    [MaxLength(128)]
    public string LotNumber { get; set; } = string.Empty;

    public ICollection<Batch> Batches { get; set; } = [];
}