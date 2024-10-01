using Agrigate.Core.Services.NotificationService;
using Agrigate.Core.Services.RuleService.Models;
using Agrigate.Domain.Contexts;
using Agrigate.Domain.Entities;
using Agrigate.Domain.Entities.Rules;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Agrigate.Core.Services.RuleService;

/// <inheritdoc />
public class RuleService : IRuleService
{
    private readonly AgrigateContext _db;
    private readonly INotificationService _notificationService;

    public RuleService(
        AgrigateContext db,
        INotificationService notificationService
    )
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _notificationService = notificationService
            ?? throw new ArgumentNullException(nameof(notificationService));
    }

    /// <inheritdoc />    
    public async Task<List<long>> GetActiveRulesForDevice(
        long deviceId,
        CancellationToken cancellationToken = default
    )
    {
        return await _db.TelemetryRules
            .AsNoTracking()
            .Where(r =>
                r.DeviceId == deviceId
                && r.IsActive
                && !r.IsDeleted
                && r.Conditions!.Count > 0
                && r.Actions!.Count > 0
            )
            .Select(r => r.Id)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<RuleValidation> ValidateTelemetryRule(
        long telemetryId, 
        long ruleId,
        CancellationToken cancellationToken = default
    )
    {
        var result = new RuleValidation();

        // Get the required entities
        var rule = await _db.TelemetryRules
            .AsNoTracking()
            .Include(r => r.Conditions)
            .Include(r => r.Actions)
            .FirstOrDefaultAsync(r => 
                r.Id == ruleId 
                && r.Conditions!.Count > 0
                && r.IsActive 
                && !r.IsDeleted, 
                cancellationToken
            ) ?? throw new ApplicationException($"Rule {ruleId} not found");

        var telemetry = await _db.Telemetry
            .FirstOrDefaultAsync(t => 
                t.Id == telemetryId
                && !t.IsDeleted
                && t.DeviceId == rule.DeviceId,
                cancellationToken
            ) ?? throw new ApplicationException($"Telemetry {telemetryId} not found for Device {rule.DeviceId}");

        // Validate each condition within the rule
        var conditionResults = new List<ConditionValidationResult>();
        foreach(var condition in rule.Conditions!)
            conditionResults.Add(await ValidateCondition(
                telemetry, condition, rule.Timespan, cancellationToken));

        // Determine if the rule has been validated
        var validated = rule.Operator == Operator.And;
        foreach(var value in conditionResults)
            validated = rule.Operator == Operator.And
                ? validated &= value.Validated
                : validated |= value.Validated;

        // Update & return the result;
        result.RuleId = rule.Id;
        result.Validated = validated;
        result.Actions = rule.Actions!.Select(a => a.Id).ToList();
        result.SourceTelemetry = conditionResults
            .SelectMany(r => r.TriggeringTelemetry)
            .Distinct()
            .ToList();

        return result;
    }

    /// <inheritdoc />
    public async Task ExecuteTelemetryAction(
        long actionId, 
        List<long> telemetryIds,
        CancellationToken cancellationToken = default
    )
    {
        // Get the required entities
        var action = await _db.TelemetryRuleActions
            .AsNoTracking()
            .Include(a => a.Rule)
            .FirstOrDefaultAsync(a =>
                a.Id == actionId
                && !a.IsDeleted
                , 
                cancellationToken
            ) ?? throw new ApplicationException($"Action {actionId} not found");

        var telemetry = await _db.Telemetry
            .Where(t => 
                telemetryIds.Contains(t.Id)
                && !t.IsDeleted
                && t.DeviceId == action.Rule!.DeviceId
            )
            .ToListAsync(cancellationToken) 
            ?? throw new ApplicationException($"Triggering telemetry not found for Device {action.Rule!.DeviceId}");

        // Get the action's definition and execute
        switch(action.Type)
        {
            case RuleAction.Notification:
                var notificationDefinition = JsonConvert
                    .DeserializeObject<NotificationDefinition>(action.Definition)
                    ?? throw new ApplicationException("Unable to retrieve action definition");

                // TODO: Implement Email & SMS
                if (
                    notificationDefinition.Channel == NotificationChannel.Email
                    || notificationDefinition.Channel == NotificationChannel.SMS
                )
                    throw new NotImplementedException();

                var title = $"{action.Rule!.Name} triggered";
                var message = $"{notificationDefinition.Content} - ";
                foreach(var item in telemetry)
                    message += $"{item.Key}: {item.Value}, ";

                // Remove the last two characters - ", "
                message = message[..^2];

                await _notificationService.SendMqttNotification(
                    notificationDefinition.Address, 
                    $"{title}: {message}", 
                    cancellationToken
                );
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Takes a rule condition and the triggering telemetry and applies the
    /// condition's logic, seeing if the condition has been validated
    /// </summary>
    /// <param name="telemetry">The incomign telemetry that triggered the 
    /// rule</param>
    /// <param name="condition">The condition to check</param>
    /// <param name="timespanInSeconds">The timespan within which telemetry 
    /// should have been received in order to validate the rule</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task<ConditionValidationResult> ValidateCondition(
        Telemetry telemetry, 
        TelemetryRuleCondition condition,
        int timespanInSeconds,
        CancellationToken cancellationToken = default
    )
    {
        bool result = false;

        // First, see if we need to validate against the incoming telemetry or
        // other telemetry from the same device
        var telemetryToCheck = new List<Telemetry>();
        if (condition.Key == telemetry.Key)
            telemetryToCheck.Add(telemetry);
        else
        {
            var now = DateTimeOffset.UtcNow;
            var lowerRange = now.AddSeconds(-timespanInSeconds);
            var otherDeviceTelemetry = await _db.Telemetry
                .AsNoTracking()
                .Where(t =>
                    t.DeviceId == telemetry.DeviceId
                    && t.Key == condition.Key
                    && !t.IsDeleted
                    && t.Timestamp >= lowerRange
                )
                .ToListAsync(cancellationToken);
            telemetryToCheck.AddRange(otherDeviceTelemetry);
        }

        // Next, get the condition's definition and see if it has been met
        switch(condition.Type)
        {
            case RuleCondition.UpperLimit:
                var upperLimit = JsonConvert
                    .DeserializeObject<UpperLimitDefinition>(condition.Definition);

                if (
                    upperLimit != null 
                    && telemetryToCheck.Any(t => t.Value >= upperLimit.Value)
                )
                    result = true;
                break;
            
            case RuleCondition.LowerLimit:
                var lowerLimit = JsonConvert
                    .DeserializeObject<LowerLimitDefinition>(condition.Definition);

                if (
                    lowerLimit != null 
                    && telemetryToCheck.Any(t => t.Value <= lowerLimit.Value)
                )
                    result = true;
                break;
            
            case RuleCondition.Range:
                var range = JsonConvert
                    .DeserializeObject<RangeDefinition>(condition.Definition);

                if (
                    range != null
                    && telemetryToCheck.Any(t =>
                        t.Value <= range.UpperLimit
                        && t.Value >= range.LowerLimit
                    )
                )
                    result = true;
                break;

            default:
                break;
        }

        return new ConditionValidationResult
        {
            Validated = result,
            TriggeringTelemetry = telemetryToCheck.Select(t => t.Id).ToList()
        };
    }
}