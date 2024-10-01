using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agrigate.Domain.Entities.Rules;

/// <summary>
/// Specifies a single condition for a Telemetry Rule
/// </summary>
[Table(nameof(TelemetryRuleCondition))]
public class TelemetryRuleCondition : EntityBase
{
    /// <summary>
    /// Primary key
    /// </summary>
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// The rule to which the condition applies
    /// </summary>
    [ForeignKey(nameof(Rule))]
    public long RuleId { get; set; }

    /// <summary>
    /// The type of condition
    /// </summary>
    public RuleCondition Type { get; set; }

    /// <summary>
    /// The telemetry key to which th ecoondition applies
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// A json string representing the defnition for this condition
    /// </summary>
    public string Definition { get; set; } = "{}";

    // Relations
    public TelemetryRule? Rule { get; set; }
}