using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agrigate.Domain.Entities.Rules;

/// <summary>
/// Specifies a single action for a Telemetry Rule
/// </summary>
[Table(nameof(TelemetryRuleAction))]
public class TelemetryRuleAction : EntityBase
{
    /// <summary>
    /// Primary key
    /// </summary>
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// The rule to which the action applies
    /// </summary>
    [ForeignKey(nameof(Rule))]
    public long RuleId { get; set; }

    /// <summary>
    /// The type of action
    /// </summary>
    public RuleAction Type { get; set; }

    /// <summary>
    /// A json string representing the definition for this action
    /// </summary>
    public string Definition { get; set; } = "{}";

    // Relations
    public TelemetryRule? Rule { get; set; }
}