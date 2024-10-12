using Agrigate.Api.Core;
using Agrigate.Core.Services.RuleService;
using Microsoft.AspNetCore.Mvc;

namespace Agrigate.Api.Controllers;

[Route("Rules")]
public class RuleController : AgrigateController
{
    private readonly IRuleService _ruleService;

    public RuleController(IRuleService ruleService)
    {
        _ruleService = ruleService
            ?? throw new ArgumentNullException(nameof(ruleService));
    }

    /// <summary>
    /// Retrieves a list of rule condition definitions
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("Conditions")]
    public async Task<IActionResult> GetRuleConditionDefinitions(
        CancellationToken cancellationToken = default
    )
    {
        try 
        {
            var result = await _ruleService
                .GetRuleConditionDefinitions(cancellationToken);

            return Success(result);
        }
        catch (Exception ex)
        {
            return Failure(ex.Message);
        }
    }

    /// <summary>
    /// Retrieves a list of rule action definitions
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("Actions")]
    public async Task<IActionResult> GetRuleActionDefinitions(
        CancellationToken cancellationToken = default
    )
    {
        try 
        {
            var result = await _ruleService
                .GetRuleActionDefinitions(cancellationToken);
            
            return Success(result);
        }
        catch (Exception ex)
        {
            return Failure(ex.Message);
        }
    }
}