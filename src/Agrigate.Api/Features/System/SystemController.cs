using Microsoft.AspNetCore.Mvc;

namespace Agrigate.Api.Features.System;

/// <summary>
/// Controller responsible for system-related logic
/// </summary>
[ApiController]
[Route("[controller]")]
public class SystemController : ControllerBase
{
    public SystemController()
    {
    }

    // TODO: Make this available to docker only so web service can depend on api
    [HttpGet("Healthcheck")]
    public IActionResult Healthcheck()
    {
        return Ok();
    }
}