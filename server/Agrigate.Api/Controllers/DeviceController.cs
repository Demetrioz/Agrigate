using Agrigate.Api.Core;
using Agrigate.Api.Validators;
using Agrigate.Core.Services.DeviceService;
using Agrigate.Core.Services.DeviceService.Models;
using Agrigate.Core.Services.RuleService;
using Agrigate.Core.Services.RuleService.Models;
using Microsoft.AspNetCore.Mvc;

namespace Agrigate.Api.Controllers;

/// <summary>
/// Controller for handling device-related requests
/// </summary>
[Route("Devices")]
public class DeviceController : AgrigateController
{
    private readonly IDeviceService _deviceService;
    private readonly IRuleService _ruleService;

    public DeviceController(
        IDeviceService deviceService,
        IRuleService ruleService
    )
    {
        _deviceService = deviceService 
            ?? throw new ArgumentNullException(nameof(deviceService));
        _ruleService = ruleService
            ?? throw new ArgumentNullException(nameof(ruleService));
    }

    /// <summary>
    /// Creates a new device within the agrigate system
    /// </summary>
    /// <param name="device"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateDevice(
        [FromBody] DeviceBase device,
        CancellationToken cancellationToken = default
    )
    {
        try 
        {
            var validation = new CreateDeviceValidator()
                .Validate(device);

            if (!validation.IsValid)
                throw new ApplicationException(validation.ToString(", "));

            var result = await _deviceService.InsertDevice(
                device, 
                cancellationToken
            );

            return Success(result);
        }
        catch (Exception ex)
        {
            return Failure(ex.Message);
        }
    }

    /// <summary>
    /// Retrieves all devices registered with Agrigate
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetDevices(
        CancellationToken cancellationToken = default
    )
    {
        try 
        {
            var result = await _deviceService.GetDevices(cancellationToken);

            return Success(result);
        }
        catch (Exception ex)
        {
            return Failure(ex.Message);
        }
    }

    /// <summary>
    /// Retrieves detailed information for a specific device
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDeviceDetails(
        long id,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var result = await _deviceService.GetDeviceDetails(
                id, 
                cancellationToken
            );

            return Success(result);
        }
        catch (Exception ex)
        {
            return Failure(ex.Message);
        }
    }

    /// <summary>
    /// Creates a new rule for the specified device
    /// </summary>
    /// <param name="id"></param>
    /// <param name="rules">The rule to create</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    [HttpPost("{id}/Rules")]
    public async Task<IActionResult> CreateDeviceRule(
        long id,
        [FromBody] DeviceRules rules,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var validation = new CreateDeviceRuleValidator(id).Validate(rules);

            if (!validation.IsValid)
                throw new ApplicationException(validation.ToString(", "));

            var result = await _ruleService.CreateDeviceRules(
                rules, 
                cancellationToken
            );

            return Success(result);
        }
        catch (Exception ex)
        {
            return Failure(ex.Message);
        }
    }

    /// <summary>
    /// Retrieves telemetry for the specified device
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id}/Telemetry")]
    public async Task<IActionResult> GetDeviceTelemetry(
        long id,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var result = await _deviceService.GetDeviceTelemetry(
                id,
                cancellationToken
            );
            return Success(result);
        }
        catch (Exception ex)
        {
            return Failure(ex.Message);
        }
    }
}