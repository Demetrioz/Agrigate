namespace Agrigate.Core.Services.DeviceService.Models;

/// <summary>
/// Basic information about a device's telemetry rule
/// </summary>
public class RuleBase
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}