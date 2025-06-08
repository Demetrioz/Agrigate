using Agrigate.Core;
using Agrigate.Core.Configuration;
using Agrigate.Core.Extensions;
using Agrigate.Domain.Contexts;
using Akka.Hosting;
using Agrigate.Engine;
using Agrigate.Engine.System.Actors;
using Akka.Logger.Serilog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var hostBuilder = new HostBuilder();
hostBuilder.ConfigureAppConfiguration((context, builder) =>
{
    builder.AddEnvironmentVariables();
});

hostBuilder.ConfigureAgrigateLogging();

hostBuilder.ConfigureServices((context, services) =>
{
    var settings = new AgrigateConfiguration();
    context.Configuration.Bind(Constants.Agrigate.Configuration, settings);

    services.AddDbContext<AgrigateContext>(options =>
        options.UseNpgsql(settings.DbConnectionString));
    
    services.AddAkka("Agrigate", (builder, sp) =>
    {
        builder.ConfigureLoggers(config =>
        {
            config.ClearLoggers();
            config.AddLogger<SerilogLogger>();
        });
        
        builder
            .WithActors((system, registry, resolver) =>
            {
                var systemProps = resolver.Props<SystemManager>();
                var systemActor = system.ActorOf(systemProps, nameof(SystemManager));
                registry.Register<SystemManager>(systemActor);
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