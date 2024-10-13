using System.ComponentModel.DataAnnotations.Schema;

namespace Agrigate.Domain.Entities.Crops;

/// <summary>
/// Represents a profile for a particular crop
/// </summary>
[Table(nameof(CropProfile))]
public class CropProfile : EntityBase
{
    /// <summary>
    /// Primary Key
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// The name of the crop
    /// </summary>
    public string Crop { get; set; } = string.Empty;
    
    /// <summary>
    /// The name of the cultivar
    /// </summary>
    public string? Cultivar { get; set; }

    /// <summary>
    /// The base temperature at which the crop will grow
    /// </summary>
    public double? BaseTemp { get; set; }

    /// <summary>
    /// The max temperature that the crop can handle
    /// </summary>
    public double? MaxTemp { get; set; }

    /// <summary>
    /// The optimal temperature that promotes growth
    /// </summary>
    public double? OptimalTemp { get; set; }

    /// <summary>
    /// The preferred way to plant the crop
    /// </summary>
    public PlantingType? PlantingType { get; set; }

    /// <summary>
    /// The growing degree days until maturity
    /// </summary>
    public int? GDD { get; set; }

    /// <summary>
    /// The days until maturity
    /// </summary>
    public int? DTM { get; set; }

    /// <summary>
    /// The number of days the crop should be grown prior to being transplanted
    /// </summary>
    public int? DaysToTransplant { get; set; }

    /// <summary>
    /// The optimal PH for the crop
    /// </summary>
    public double? PH { get; set; }

    /// <summary>
    /// The optimal EC for the crop
    /// </summary>
    public double? EC { get; set; }

    /// <summary>
    /// The optimal in-row spacing between plants
    /// </summary>
    public double? Spacing { get; set; }

    /// <summary>
    /// The optimal spacing between rows
    /// </summary>
    public double? RowSpacing { get; set; }
}