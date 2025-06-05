namespace Agrigate.Domain.Models;

/// <summary>
/// Base information from a CropDetail record 
/// </summary>
/// <param name="Crop">The name of the crop</param>
/// <param name="Cultivar">The name of the cultivar</param>
/// <param name="Dtm">The number of days to maturity</param>
/// <param name="StackingDays">The number of days the crop should be stacked</param>
/// <param name="BlackoutDays">The number of days the crop shoudl remain in blackout</param>
/// <param name="NurseryDays">The number of days the crop should remain in the nursery</param>
/// <param name="SoakHours">The number of hours seed should be soaked prior to planting</param>
/// <param name="PlantQuantity">The amount of seed to use when planting</param>
public record CropDetailBase(
    string Crop,
    string? Cultivar,
    int Dtm,
    int? StackingDays,
    int? BlackoutDays,
    int? NurseryDays,
    int? SoakHours,
    double? PlantQuantity
);
