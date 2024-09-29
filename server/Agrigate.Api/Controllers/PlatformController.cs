using System.Reflection;
using Agrigate.Api.Actors;
using Agrigate.Api.Models;
using Agrigate.Domain.Messages;
using Akka.Actor;
using Akka.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Agrigate.Api.Controllers;

[ApiController]
[Route("Platform")]
public class PlatformController : ControllerBase
{
    private readonly IActorRef _supervisor;

    public PlatformController(IRequiredActor<ApiSupervisor> supervisor)
    {
        _supervisor = supervisor.ActorRef 
            ?? throw new ArgumentNullException(nameof(supervisor));
    }

    [HttpGet("Version")]
    public async Task<IActionResult> GetPlatformVersion(
        CancellationToken cancellationToken = default
    )
    {
        var result = new PlatformVersions();

        try
        {
            var apiVersion = Assembly.GetEntryAssembly()
                ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion ?? "0.0.0";

            var eventServiceVersion = await _supervisor.Ask(
                new GetServiceVersion(),
                TimeSpan.FromSeconds(5),
                cancellationToken
            );

            result.Api = apiVersion;
            result.EventService = (string)eventServiceVersion;
        }
        catch {}

        return new OkObjectResult(result);
    }
}