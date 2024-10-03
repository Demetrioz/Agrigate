namespace Agrigate.EventService.Messages;

/// <summary>
/// Used to activate the RuleEngine when new telemetry has been received
/// </summary>
public record ActivateEngine(
    long DeviceId,
    long TelemetryId
);

/// <summary>
/// Used to confirm whether a rule exists and can be utilized
/// </summary>
/// <param name="DeviceId">The id of the device that sent telemetry</param>
/// <param name="TelemetryId">The id of the telemetry that was received</param>
public record ConfirmRules(
    long DeviceId,
    long TelemetryId
);

/// <summary>
/// Results returned from confirming whether there are rules associated with
/// a particular device
/// </summary>
/// <param name="RuleIds">The rule Ids associated with the device</param>
/// <param name="TelemetryId">The telemetry that triggered the rule</param>
/// <param name="Exception">Any exception from the confirmation task</param>
public record ConfirmationResult(
    List<long> RuleIds,
    long TelemetryId,
    Exception? Exception = null
);

/// <summary>
/// Used to confirm whether a rule is valid for a given telemetry value
/// </summary>
/// <param name="RuleId">The id of the rule to validate</param>
/// <param name="TelemetryId">The telemetry that triggered the rule</param>
public record ValidateRule(
    long RuleId,
    long TelemetryId
);

/// <summary>
/// Results from validation of a rule
/// </summary>
/// <param name="Validated">Whether the rule has been validated</param>
/// <param name="ActionIds">Ids of actions that should be performed if the 
/// rule has been validated</param>
/// <param name="TelemetryIds">Telemetry that caused the rule to be 
/// valid</param>
/// <param name="Exception"></param>
public record ValidationResult(
    bool Validated,
    List<long> ActionIds,
    List<long> TelemetryIds,
    Exception? Exception = null
);

/// <summary>
/// Used to execute a given action due to the values of telemetry
/// </summary>
/// <param name="ActionId">The id of the action to execute</param>
/// <param name="TelemetryIds">The telemetry that caused the action to 
/// execute</param>
public record InitiateAction(
    long ActionId,
    List<long> TelemetryIds
);

/// <summary>
/// Results from an action execution task
/// </summary>
/// <param name="Exception"></param>
public record ActionResult(
    Exception? Exception = null
);