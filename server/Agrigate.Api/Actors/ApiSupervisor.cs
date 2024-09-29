using Agrigate.Api.Configuration;
using Agrigate.Core.Actors;
using Agrigate.Domain.Messages;
using Akka.Actor;
using Akka.Event;
using Microsoft.Extensions.Options;

namespace Agrigate.Api.Actors;

/// <summary>
/// The root actor for the Api
/// </summary>
public class ApiSupervisor : AgrigateActor
{
    private readonly ApiOptions _options;
    private ActorSelection? _eventService;

    public ApiSupervisor(IOptions<ApiOptions> options) : base()
    {
        _options = options.Value
            ?? throw new ArgumentNullException(nameof(options));

        Receive<GetServiceVersion>(RetrieveServiceVersions);
    }

    protected override void PreStart()
    {
        Logger.Info("{0} starting...", nameof(ApiSupervisor));

        InstantiateRemoteSupervisors();

        Logger.Info("{0} ready!", nameof(ApiSupervisor));
    }

    /// <summary>
    /// Creates an actor selector to communicate with other Agrigate services
    /// </summary>
    private void InstantiateRemoteSupervisors()
    {
        Logger.Info("{0} running...", nameof(InstantiateRemoteSupervisors));

        _eventService = Context.ActorSelection($"akka.tcp://{_options.EventService.ServiceName}@{_options.EventService.Hostname}:{_options.EventService.Port}/user/{_options.EventService.SupervisorName}");

        Logger.Info("{0} completed!", nameof(InstantiateRemoteSupervisors));
    }

    /// <summary>
    /// Retrieves version numbers from the other Agrigate services
    /// </summary>
    /// <param name="message"></param>
    private void RetrieveServiceVersions(GetServiceVersion message)
    {
        _eventService
            ?.Ask(message, TimeSpan.FromSeconds(3))
            .PipeTo(Sender);
    }
}