namespace Agrigate.Core.Services.TelemetryService.Models;

/// <summary>
/// A base telemetry object sent to an MQTT broker by a device
/// </summary>
public class TelemetryBase
{
    public long DeviceId { get; set; }
    public string Key { get; set; } = string.Empty;
    public double Value { get; set; }
    public string? Unit { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
}