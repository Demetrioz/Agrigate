using Agrigate.Core.Services.DeviceService;
using Agrigate.Core.Services.DeviceService.Models;
using Microsoft.AspNetCore.Mvc;

namespace Agrigate.Api.Controllers;

/// <summary>
/// Controller for handling device-related requests
/// </summary>
[ApiController]
[Route("Devices")]
public class DeviceController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DeviceController(IDeviceService deviceService)
    {
        _deviceService = deviceService 
            ?? throw new ArgumentNullException(nameof(deviceService));
    }

    /// <summary>
    /// Creates a new device within the agrigate system
    /// </summary>
    /// <param name="device"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateDevice([FromBody] DeviceBase device)
    {
        try 
        {
            var result = await _deviceService.InsertDevice(device);
            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(new { Error = ex.Message });
        }
    }
}