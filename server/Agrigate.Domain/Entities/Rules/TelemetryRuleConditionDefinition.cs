using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agrigate.Domain.Entities.Rules;

/// <summary>
/// Defines the conditions for a Telemetry Rule
/// </summary>
[Table(nameof(TelemetryRuleConditionDefinition))]
public class TelemetryRuleConditionDefinition : EntityBase
{
    /// <summary>
    /// Primary key
    /// </summary>
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// The type of condition being defined
    /// </summary>
    public RuleCondition Type { get; set; }

    /// <summary>
    /// A json string representing the definition for the condition
    /// </summary>
    public string Definition { get; set; } = "{}";
}