using Akka.Hosting;
using Agrigate.Core;
using Agrigate.Core.Actors.Root;
using Agrigate.Domain.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var hostBuilder = new HostBuilder();
hostBuilder.ConfigureHostConfiguration(builder =>
        builder.AddEnvironmentVariables().AddCommandLine(args));

hostBuilder.ConfigureServices((context, services) =>
{
    services.AddAgrigateDb(context.Configuration);
    
    services.AddAkka("MyActorSystem", (builder, sp) =>
    {
        builder
            .WithActors((system, registry, resolver) =>
            {
                var coreProps = resolver.Props<CoreManager>();
                var coreActor = system.ActorOf(coreProps, "core-manager");
                registry.Register<CoreManager>(coreActor);
            })
            .WithActors((system, registry, resolver) =>
            {
                var helloActor = system.ActorOf(Props.Create(() => new HelloActor()), "hello-actor");
                registry.Register<HelloActor>(helloActor);
            })
            .WithActors((system, registry, resolver) =>
            {
                var timerActorProps =
                    resolver.Props<TimerActor>(); // uses Msft.Ext.DI to inject reference to helloActor
                var timerActor = system.ActorOf(timerActorProps, "timer-actor");
                registry.Register<TimerActor>(timerActor);
            });
    });
});

var host = hostBuilder.Build();

await host.RunAsync();