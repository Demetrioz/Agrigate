namespace Agrigate.Core.Services.RuleService.Models;

/// <summary>
/// A object containing pertinent information regarding whether or not a rule's
/// condition has been validated
/// </summary>
public class ConditionValidationResult
{
    public bool Validated { get; set; }
    public List<long> TriggeringTelemetry { get; set; } = [];
}