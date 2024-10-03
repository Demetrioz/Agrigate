using Agrigate.Core.Actors;
using Agrigate.Core.Services.RuleService;
using Agrigate.EventService.Messages;
using Akka.Actor;
using Akka.Event;

namespace Agrigate.EventService.Actors.Rules;

/// <summary>
/// Actor responsible for executing any actions associated with a validated
/// rule
/// </summary>
public class RuleActionActor : AgrigateActor
{
    private readonly IServiceProvider _provider;

    public RuleActionActor(IServiceProvider provider)
    {
        _provider = provider
            ?? throw new ArgumentNullException(nameof(provider));

        ReceiveAsync<InitiateAction>(HandleAction);
    }

    /// <summary>
    /// Executes the action for a validated rule
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private async Task HandleAction(InitiateAction message)
    {
        Exception? exception = null;

        try
        {
            using var scope = _provider.CreateScope();
            var ruleService = scope.ServiceProvider
                .GetRequiredService<IRuleService>();

            await ruleService.ExecuteTelemetryAction(
                message.ActionId,
                message.TelemetryIds
            );
        }
        catch (Exception ex)
        {
            Logger.Error(
                "Error executing action {0}: {1}",
                message.ActionId,
                ex.Message
            );

            exception = ex;
        }

        Sender.Tell(new ActionResult(
            exception
        ));
    }
}