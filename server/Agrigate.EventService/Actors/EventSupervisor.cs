using System.Reflection;
using Agrigate.Core.Actors;
using Agrigate.Domain.Messages;
using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Event;

namespace Agrigate.EventService.Actors;

/// <summary>
/// The root actor for the Event Service. 
/// </summary>
public class EventSupervisor : AgrigateActor
{
    private IActorRef? _telemetryHandler;

    public EventSupervisor()
    {
        Receive<GetServiceVersion>(ReturnServiceVersion);
    }

    protected override void PreStart()
    {
        Logger.Info("{0} starting...", nameof(EventSupervisor));

        CreateChildren();

        Logger.Info("{0} ready!", nameof(EventSupervisor));
    }

    /// <summary>
    /// Creates child actors that are responsible for handling different events
    /// from the MQTT Broker
    /// </summary>
    private void CreateChildren()
    {
        Logger.Info("{0} running...", nameof(CreateChildren));

        var telemetryProps = DependencyResolver
            .For(Context.System)
            .Props<TelemetryHandler>();

        _telemetryHandler = Context
            .ActorOf(telemetryProps, nameof(TelemetryHandler));

        Logger.Info("{0} completed!", nameof(CreateChildren));
    }

    /// <summary>
    /// Returns the EventService version to the requestor
    /// </summary>
    /// <param name="message"></param>
    private void ReturnServiceVersion(GetServiceVersion message)
    {
        var serviceVersion = Assembly.GetEntryAssembly()
                ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion ?? "0.0.0";

        Sender.Tell(serviceVersion);
    }
}