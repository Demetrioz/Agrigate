using Agrigate.EventService.Actors;
using Agrigate.EventService.Configuration;
using Akka.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<TelemetryOptions>(
    builder.Configuration.GetSection("Telemetry"));

builder.Services.AddAkka(nameof(Agrigate.EventService), builder =>
{
    builder
        .WithActors((system, registry, resolver) =>
        {
            var supervisorProps = resolver.Props<EventSupervisor>();
            var supervisor = system.ActorOf(
                supervisorProps, 
                nameof(EventSupervisor)
            );

            registry.Register<EventSupervisor>(supervisor);
        });
});

var host = builder.Build();
host.Run();
