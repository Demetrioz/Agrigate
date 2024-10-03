namespace Agrigate.Core.Services.RuleService.Models;

/// <summary>
/// An object containing pertinent information about whether a particular rule
/// has been validated
/// </summary>
public class RuleValidation
{
    public long RuleId { get; set; }
    public bool Validated { get; set; }
    public List<long> Actions { get; set; } = [];
    public List<long> SourceTelemetry { get; set; } = [];
}