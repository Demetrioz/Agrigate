using Agrigate.Core.Actors;
using Agrigate.Core.Services.RuleService;
using Agrigate.EventService.Messages;
using Akka.Actor;
using Akka.Event;

namespace Agrigate.EventService.Actors.Rules;

/// <summary>
/// Actor responsible for validating whether a rule associated with a device has
/// met the criteria to activate the associated actions
/// </summary>
public class RuleValidationActor : AgrigateActor
{
    private readonly IServiceProvider _provider;

    public RuleValidationActor(IServiceProvider provider)
    {
        _provider = provider
            ?? throw new ArgumentNullException(nameof(provider));

        ReceiveAsync<ValidateRule>(HandleRuleValidation);
    }

    /// <summary>
    /// Validates all rules for a given device
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    private async Task HandleRuleValidation(ValidateRule message)
    {
        var validated = false;
        var actions = new List<long>();
        var sourceTelemetry = new List<long>();
        Exception? exception = null;

        try
        {
            using var scope = _provider.CreateScope();
            var ruleService = scope.ServiceProvider
                .GetRequiredService<IRuleService>();

            var validationResult = await ruleService
                .ValidateTelemetryRule(message.TelemetryId, message.RuleId);

            if (validationResult.Validated) 
            {
                validated = true;
                actions = validationResult.Actions;
                sourceTelemetry = validationResult.SourceTelemetry;
            }
        }
        catch (Exception ex)
        {
            Logger.Error(
                "Error validating rules {0}: {1}",
                string.Join(", ", message.RuleId),
                ex.Message
            );

            exception = ex;
        }

        Sender.Tell(new ValidationResult(
            validated, 
            actions,
            sourceTelemetry,
            exception
        ));
    }
}