using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agrigate.Domain.Entities;

/// <summary>
/// Represents a physical device
/// </summary>
[Table(nameof(Device))]
public class Device : EntityBase
{
    /// <summary>
    /// Primary key
    /// </summary>
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// A friendly name for the device
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The current location of the device
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// Telemetry received by the device
    /// </summary>
    public ICollection<Telemetry>? Telemetry { get; set; }
}