using Agrigate.Domain.Entities.Rules;

namespace Agrigate.Core.Services.RuleService.Models;

/// <summary>
/// A condition that should be met before a rule is executed
/// </summary>
public class CreateRuleCondition
{
    /// <summary>
    /// The telemetry key that the condition applies to
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// The type of condition
    /// </summary>
    public RuleCondition Type { get; set; }

    /// <summary>
    /// A json string containing the condition's definition
    /// </summary>
    public string Definition { get; set; } = "{}";
}

/// <summary>
/// An action that should be executed as part of a rule
/// </summary>
public class CreateRuleAction
{
    /// <summary>
    /// The type of action
    /// </summary>
    public RuleAction Type { get; set; }
    
    /// <summary>
    /// A json string containing the action's definition
    /// </summary>
    public string Definition { get; set; } = "{}";
}

/// <summary>
/// A definition of a rule that should be created for a device
/// </summary>
public class RuleDefinition
{
    /// <summary>
    /// The name of the rule
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// An operator for how the conditions should be evaluated, if there are
    /// more than one
    /// </summary>
    public Operator Operator { get; set; }

    /// <summary>
    /// The timespan within which to look when verifiying the rule's conditions
    /// </summary>
    public int Timespan { get; set; }

    /// <summary>
    /// Whether or not the rule is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// A list of conditions that must be satified before actions are taken
    /// </summary>
    public List<CreateRuleCondition> Conditions { get; set; } = [];

    /// <summary>
    /// A list of actions to take once the conditions have been met
    /// </summary>
    public List<CreateRuleAction> Actions { get; set; } = [];
}

/// <summary>
/// Body of a request made to the /devices/{id}/rules endpoint to add rules
/// to a device
/// </summary>
public class DeviceRules
{
    /// <summary>
    /// The Id of the device for which a rule should be created
    /// </summary>
    public long DeviceId { get; set; }

    /// <summary>
    /// A list of rules to create for the device
    /// </summary>
    public List<RuleDefinition> Rules { get; set; } = [];
}