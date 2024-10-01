using Agrigate.Core.Actors;
using Agrigate.Core.Services.RuleService;
using Agrigate.EventService.Messages;
using Akka.Actor;
using Akka.Event;

namespace Agrigate.EventService.Actors.Rules;

/// <summary>
/// Actor responsible for checking whether or not a rule exists for a given
/// device
/// </summary>
public class RuleConfirmationActor : AgrigateActor
{
    private readonly IServiceProvider _provider;

    public RuleConfirmationActor(IServiceProvider provider)
    {
        _provider = provider 
            ?? throw new ArgumentNullException(nameof(provider));

        ReceiveAsync<ConfirmRules>(HandleRuleConfirmation);
    }

    /// <summary>
    /// Returns any rules associated with a device
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private async Task HandleRuleConfirmation(ConfirmRules message)
    {
        var ruleIds = new List<long>();
        Exception? exception = null;

        try
        {
            using var scope = _provider.CreateScope();
            var ruleService = scope.ServiceProvider
                .GetRequiredService<IRuleService>();

            ruleIds = await ruleService
                .GetActiveRulesForDevice(message.DeviceId);
        }
        catch (Exception ex)
        {
            Logger.Error(
                "Error confirming rules for device {0}: {1}", 
                message.DeviceId, 
                ex.Message
            );
            
            exception = ex;
        }

        Sender.Tell(new ConfirmationResult(
            ruleIds, 
            message.TelemetryId, 
            exception
        ));
    }
}