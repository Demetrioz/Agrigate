using Agrigate.Api.Models.Requests;
using Agrigate.Domain.Entities.Rules;
using FluentValidation;
using Newtonsoft.Json;

namespace Agrigate.Api.Validators;

public class CreateDeviceRuleValidator : AbstractValidator<DeviceRules>
{
    public CreateDeviceRuleValidator(long deviceId)
    {
        RuleFor(x => x.DeviceId).NotNull().GreaterThan(0).WithMessage("DeviceId is required and must be greater than 0");
        RuleFor(x => x.DeviceId).Equal(deviceId).WithMessage("Non-matching DeviceId");
        RuleFor(x => x.Rules).NotNull().NotEmpty().WithMessage("At least one rule is required");
        RuleFor(x => x.Rules).Must((root, rules, context) => 
        {
            // Whether the rules are valid
            var result = true;
            // The first index of an invalid rule
            var index = 0;
            // The issue with the rule at index
            var message = "";

            // Complete the validation logic
            for (var i = 0; i < rules.Count; i++)
            {
                var (indexResult, reason) = IsAValidRule(rules[i]);
                if (indexResult == false)
                {
                    result = false;
                    index = i;
                    message = reason;
                    break;
                }
            }

            // Append custom arguments
            context.MessageFormatter
                .AppendArgument("Index", index)
                .AppendArgument("Message", message);

            // Return the result
            return result;
        })
        .WithMessage("The rule at {Index} has the following error: {Message}");
    }

    private (bool, string) IsAValidRule(RuleDefinition rule)
    {
        if (string.IsNullOrWhiteSpace(rule.Name))
            return (false, "Rules require a name");

        if (rule.Conditions.Count == 0)
            return (false, "Rules must have at least one Condition");

        if (rule.Actions.Count == 0)
            return (false, "Rules must have at least one Action");

        foreach(var condition in rule.Conditions)
        {
            var (result, reason) = IsAValidCondition(condition);
            if (result == false)
                return (false, reason);
        }

        foreach(var action in rule.Actions)
        {
            var (result, reason) = IsAValidAction(action);
            if (result == false)
                return (false, reason);
        }

        return (true, "");
    }

    private (bool, string) IsAValidCondition(CreateRuleCondition condition)
    {
        var result = true;
        var reason = "";

        try
        {
            if (string.IsNullOrWhiteSpace(condition.Key))
                throw new ApplicationException("Conditions must have a Key");

            switch (condition.Type)
            {
                case RuleCondition.UpperLimit:
                    var upperLmiit = JsonConvert
                        .DeserializeObject<UpperLimitDefinition>(condition.Definition)
                        ?? throw new ApplicationException("Invalid UpperLimit Definition");
                    break;

                case RuleCondition.LowerLimit:
                    var lowerLimit = JsonConvert
                        .DeserializeObject<LowerLimitDefinition>(condition.Definition)
                        ?? throw new ApplicationException("Invalid LowerLimit Definition");
                    break;

                case RuleCondition.Range:
                    var range = JsonConvert
                        .DeserializeObject<RangeDefinition>(condition.Definition)
                        ?? throw new ApplicationException("Invalid Range Definition");
                    break;

                default:
                    break;
            }
        }
        catch
        {
            result = false;
            reason = "Invalid Condition Definition";
        }

        return (result, reason);
    }

    private (bool, string) IsAValidAction(CreateRuleAction action)
    {
        var result = true;
        var reason = "";

        try
        {
            switch (action.Type)
            {
                case RuleAction.Notification:
                    var definition = JsonConvert
                        .DeserializeObject<NotificationDefinition>(action.Definition)
                        ?? throw new ApplicationException("Invalid Action Definition");
                    break;
                
                default:
                    break;
            }
        }
        catch
        {
            result = false;
            reason = "Invalid Action Definition";
        }

        return (result, reason);
    }
}