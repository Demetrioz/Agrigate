using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agrigate.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    // private readonly ILogger<WeatherForecastController> _logger;
    
    // public WeatherForecastController(ILogger<WeatherForecastController> logger)
    // {
    //     _logger = logger;
    // }

    [HttpPost]
    public IActionResult Test()
    {
        // _logger.LogInformation("Test Info");
        // _logger.LogError("Test Error");
        // _logger.LogCritical("Test Critical");
        // _logger.LogWarning("Test Warning");
        return Ok();
    }

    [Authorize]
    [HttpPost("Test2")]
    public IActionResult Test2()
    {
        var claims = User.Claims
            .Select(c => new { c.Type, c.Value })
            .ToList();
        
        return Ok(claims);
    }

    [Authorize(Roles = "api-admin")]
    [HttpPost("Test3")]
    public IActionResult Test3()
    {
        return Ok();
    }
    
    [Authorize(Roles = "agrigate-admin")]
    [HttpPost("Test4")]
    public IActionResult Test4()
    {
        return Ok();
    }
    
    [Authorize(Roles = "invalid-role")]
    [HttpPost("Test5")]
    public IActionResult Test5()
    {
        return Ok();
    }
    
    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}