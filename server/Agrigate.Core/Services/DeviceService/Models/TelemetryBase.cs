namespace Agrigate.Core.Services.DeviceService.Models;

/// <summary>
/// The minimium information required for telemetry
/// </summary>
public class TelemetryBase
{
    public long Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public double Value { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}