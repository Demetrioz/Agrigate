namespace Agrigate.Core.Services.DeviceService.Models;

/// <summary>
/// Detailed information about a specific device
/// </summary>
public class DeviceDetails
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public List<RuleBase> Rules { get; set; } = [];
    public List<TelemetryBase> DistinctTelemetry { get; set; } = [];
}