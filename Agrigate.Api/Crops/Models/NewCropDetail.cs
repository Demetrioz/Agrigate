using Agrigate.Api.Core.ValueTypes;

namespace Agrigate.Api.Crops.Models;

/// <summary>
/// Model for creating a new CropDetail record 
/// </summary>
/// <param name="Crop">The name of the crop</param>
/// <param name="Cultivar">The name of the cultivar</param>
/// <param name="Dtm">The number of days to maturity</param>
/// <param name="StackingDays">The number of days the crop should be stacked</param>
/// <param name="BlackoutDays">The number of days the crop shoudl remain in blackout</param>
/// <param name="NurseryDays">The number of days the crop should remain in the nursery</param>
/// <param name="SoakHours">The number of hours seed should be soaked prior to planting</param>
/// <param name="PlantQuantity">The amount of seed to use when planting</param>
public record NewCropDetail(
    NonEmptyString Crop,
    string? Cultivar,
    PositiveInt Dtm,
    NullOrNonNegativeInt StackingDays,
    NullOrNonNegativeInt BlackoutDays,
    NullOrNonNegativeInt NurseryDays,
    NullOrNonNegativeInt SoakHours,
    NullOrPositiveDouble PlantQuantity);