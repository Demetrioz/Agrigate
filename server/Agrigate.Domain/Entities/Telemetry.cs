using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agrigate.Domain.Entities;

/// <summary>
/// Telemetry readings received by a device
/// </summary>
[Table(nameof(Telemetry))]
public class Telemetry : EntityBase
{
    /// <summary>
    /// Primary key
    /// </summary>
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// The id of the device to which the telemetry belongs
    /// </summary>
    [ForeignKey(nameof(Device))]
    public long DeviceId { get; set; }

    /// <summary>
    /// The timestamp of the telemetry reading
    /// </summary>
    public DateTimeOffset Timestamp { get; set; }

    /// <summary>
    /// The identifier for the value being recorded
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// The value of the value received from the device
    /// </summary>
    public double Value { get; set; }

    /// <summary>
    /// The SI unit of the metric
    /// </summary>
    public string? Unit { get; set; }

    // Relations
    public Device? Device { get; set; }
}