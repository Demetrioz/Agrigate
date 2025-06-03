using System.ComponentModel.DataAnnotations;
using Agrigate.Domain.Entities.Common;

namespace Agrigate.Domain.Entities.Crops;

/// <summary>
/// Contains details about a particular crop
/// </summary>
public class CropDetail : EntityBase
{
    /// <summary>
    /// The name of the crop
    /// </summary>
    [MaxLength(255)]
    public string Crop { get; set; } = string.Empty;
    
    /// <summary>
    /// The name of the cultivar
    /// </summary>
    [MaxLength(255)]
    public string? Cultivar { get; set; }
    
    /// <summary>
    /// The number of days required until the crop can be harvested
    /// </summary>
    public int Dtm { get; set; }
    
    /// <summary>
    /// How many days the crop must be stacked. Relevant for microgreens
    /// </summary>
    public int? StackingDays { get; set; }
    
    /// <summary>
    /// How many days the crop must remain in blackout, after being stacked.
    /// Relevant for microgreens
    /// </summary>
    public int? BlackoutDays { get; set; }
    
    /// <summary>
    /// The number of days the crop should remain in the nursery before being
    /// transplanted
    /// </summary>
    public int? NurseryDays { get; set; }
    
    /// <summary>
    /// The number of hours seed should be soaked before planting. Relevant
    /// for microgreens
    /// </summary>
    public int? SoakHours { get; set; }
 
    /// <summary>
    /// The amount of seed that should be planted per tray. Relevant for
    /// microgreens
    /// </summary>
    public double? PlantQuantity { get; set; }
}