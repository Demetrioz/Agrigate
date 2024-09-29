namespace Agrigate.Core.Services.DeviceService.Models;

/// <summary>
/// The minimum information required when creating a device
/// </summary>
public class DeviceBase
{
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
}