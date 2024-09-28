using Agrigate.Core.Services.MqttService;
using Agrigate.Core.Services.TelemetryService;
using Agrigate.Domain.Configuration;
using Agrigate.Domain.Contexts;
using Agrigate.EventService.Actors;
using Agrigate.EventService.Configuration;
using Akka.Hosting;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

//////////////////////////////////////////
//          Configure Settings          //
//////////////////////////////////////////

builder.Services.Configure<TelemetryOptions>(
    builder.Configuration.GetSection("Telemetry"));

var dbOptions = new DatabaseOptions();
builder.Configuration.Bind("Database", dbOptions);

//////////////////////////////////////////
//            Database Setup            //
//////////////////////////////////////////

var connectionString = $"Host={dbOptions.Host};Port={dbOptions.Port};Database={dbOptions.Database};User Id={dbOptions.Username};Password={dbOptions.Password};";
builder.Services.AddDbContext<AgrigateContext>(options =>
    options.UseNpgsql(connectionString));

//////////////////////////////////////////
//          Configure Services          //
//////////////////////////////////////////

builder.Services
    .AddSingleton<IMqttService, MqttService>()
    .AddTransient<ITelemetryService, TelemetryService>();

//////////////////////////////////////////
//               Akka.Net               //
//////////////////////////////////////////

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
