using Agrigate.Core.Services.RuleService.Models;
using Agrigate.Domain.Entities.Rules;

namespace Agrigate.Core.Services.RuleService;

/// <summary>
/// A service for performing logic within the Rule System
/// </summary>
public interface IRuleService
{
    /// <summary>
    /// Retrieves a list of active rules for the given device
    /// </summary>
    /// <param name="deviceId">The id of the device for which rules should be
    /// retrieved</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<long>> GetActiveRulesForDevice(
        long deviceId, 
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Validates a given rule based on the telemetry received
    /// </summary>
    /// <param name="telemetryId">The telemetry that triggered the 
    /// validation</param>
    /// <param name="ruleId">The rule to validate</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<RuleValidation> ValidateTelemetryRule(
        long telemetryId, 
        long ruleId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Executes an action associated with a Telemetry rule
    /// </summary>
    /// <param name="actionId">The action that should be executed</param>
    /// <param name="telemetryIds">The telemetry that caused the action to be
    /// executed</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ExecuteTelemetryAction(
        long actionId,
        List<long> telemetryIds,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Creates Telemetry rules for a particular device
    /// </summary>
    /// <param name="request">The request DTO that contains information about the
    /// rules that should be created</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<TelemetryRule>> CreateDeviceRules(
        DeviceRules request,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Retrieves the defined rule condition definitions
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<BaseDefinition<RuleCondition>>> GetRuleConditionDefinitions(
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Retrieves the defined rule action definitions
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<BaseDefinition<RuleAction>>> GetRuleActionDefinitions(
        CancellationToken cancellationToken = default
    );
}