using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agrigate.Domain.Entities.Rules;

/// <summary>
/// A rule that applies to telemetry from a given device
/// </summary>
[Table(nameof(TelemetryRule))]
public class TelemetryRule : EntityBase
{
    /// <summary>
    /// Primary key
    /// </summary>
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// The device for which the rule applies
    /// </summary>
    [ForeignKey(nameof(Device))]
    public long DeviceId { get; set; }

    /// <summary>
    /// The name of the rule
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Whether the rule is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Whether conditions for the rule are all required ("And"ed) or any of
    /// them are required ("Or"ed)
    /// </summary>
    public Operator Operator { get; set; }

    /// <summary>
    /// The amount of time in which all conditions must have occured in order 
    /// to be considered "passing"
    /// </summary>
    public int Timespan { get; set; }

    /// <summary>
    /// Conditions that must be met before any actions are taken
    /// </summary>
    public ICollection<TelemetryRuleCondition>? Conditions { get; set; }

    /// <summary>
    /// The actions taken once any or all conditions have been met
    /// </summary>
    public ICollection<TelemetryRuleAction>? Actions { get; set; }

    // Relations
    public Device? Device { get; set; }
}