using Agrigate.Api.Crops.Models;

namespace Agrigate.Api.Crops.Messages;

/// <summary>
/// Commands to complete crop-related tasks
/// </summary>
public static class CropCommands
{
    /// <summary>
    /// Command to create a new CropDetail record
    /// </summary>
    /// <param name="Detail">The Crop Details that should be recorded</param>
    public sealed record CreateCropDetail(NewCropDetail Detail);
}