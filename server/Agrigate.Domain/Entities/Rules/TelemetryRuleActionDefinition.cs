using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agrigate.Domain.Entities.Rules;

/// <summary>
/// Defines the actions for a Telemetry Rule
/// </summary>
[Table(nameof(TelemetryRuleActionDefinition))]
public class TelemetryRuleActionDefinition : EntityBase
{
    /// <summary>
    /// Primary key
    /// </summary>
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// The type of action being defined
    /// </summary>
    public RuleAction Type { get; set; }

    /// <summary>
    /// A json string representing the definition for the action
    /// </summary>
    public string Definition { get; set; } = "{}";
}