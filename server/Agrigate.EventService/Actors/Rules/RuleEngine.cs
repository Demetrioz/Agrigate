using Agrigate.Core.Actors;
using Agrigate.EventService.Messages;
using Akka.Actor;
using Akka.DependencyInjection;

namespace Agrigate.EventService.Actors.Rules;

/// <summary>
/// Root actor for the rules engine
/// </summary>
public class RuleEngine : AgrigateActor
{
    public RuleEngine()
    {
        Receive<ActivateEngine>(HandleActivation);
        Receive<ConfirmationResult>(HandleConfirmation);
        Receive<ValidationResult>(HandleValidation);
        Receive<ActionResult>(HandleAction);
    }

    /// <summary>
    /// The first step of the process. Confirms whether or not a rule exists
    /// for the given device
    /// </summary>
    /// <param name="message"></param>
    private void HandleActivation(ActivateEngine message)
    {
        var ask = new ConfirmRules(message.DeviceId, message.TelemetryId);

        var confirmationProps = DependencyResolver
            .For(Context.System)
            .Props<RuleConfirmationActor>();

        var confirmationHandler = Context.ActorOf(
            confirmationProps, 
            $"{nameof(RuleConfirmationActor)}-{Guid.NewGuid()}"
        );

        confirmationHandler
            .Ask(ask)
            .PipeTo(Self, confirmationHandler);
    }

    /// <summary>
    /// The second step of the process. Validates whether the rule has met all
    /// requirements to be activated
    /// </summary>
    /// <param name="message"></param>
    private void HandleConfirmation(ConfirmationResult message)
    {
        // Kill the confirmation handler we received the message from
        Sender.Tell(PoisonPill.Instance);

        // Validate the rules
        if (message.RuleIds.Count > 0)
        {
            foreach(var id in message.RuleIds)
            {
                var ask = new ValidateRule(id, message.TelemetryId);
                
                var validatorProps = DependencyResolver
                    .For(Context.System)
                    .Props<RuleValidationActor>();

                var validator = Context.System.ActorOf(
                    validatorProps,
                    $"{nameof(RuleValidationActor)}-{Guid.NewGuid()}"
                );

                validator
                    .Ask(ask)
                    .PipeTo(Self, validator);
            }
        }
    }

    /// <summary>
    /// The third step of the process. Initiates the rule's actions
    /// </summary>
    /// <param name="message"></param>
    private void HandleValidation(ValidationResult message)
    {
        // Kill the validation handler we received the message from
        Sender.Tell(PoisonPill.Instance);

        // Initiate the actions
        if (message.Validated && message.ActionIds.Count > 0)
        {
            foreach(var id in message.ActionIds)
            {
                var ask = new InitiateAction(id, message.TelemetryIds);

                var actionProps = DependencyResolver
                    .For(Context.System)
                    .Props<RuleActionActor>();

                var handler = Context.System.ActorOf(
                    actionProps,
                    $"{nameof(RuleActionActor)}-{Guid.NewGuid()}"
                );

                handler
                    .Ask(ask)
                    .PipeTo(Self, handler);
            }
        }
    }

    /// <summary>
    /// The final step in the process. Handles any errors from the rule's 
    /// actions
    /// </summary>
    /// <param name="message"></param>
    private void HandleAction(ActionResult message)
    {
        // Kill the action handler we received the message from
        Sender.Tell(PoisonPill.Instance);
    }
}