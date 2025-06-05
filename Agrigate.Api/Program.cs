using System.Reflection;
using Agrigate.Api.Crops.Actors;
using Agrigate.Api.System.Actors;
using Agrigate.Core;
using Agrigate.Core.Configuration;
using Agrigate.Core.Extensions;
using Agrigate.Domain.Contexts;
using Akka.Hosting;
using Akka.Logger.Serilog;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureAgrigateLogging(builder.Configuration);

var settings = new AgrigateConfiguration();
builder.Configuration.Bind(Constants.Agrigate.Configuration, settings);

builder.Services.AddDbContext<AgrigateContext>(options =>
    options.UseNpgsql(settings.DbConnectionString));

builder.Services.AddAkka("Agrigate", (akkaBuilder, provider) =>
{
    akkaBuilder.ConfigureLoggers(config =>
    {
        config.ClearLoggers();
        config.AddLogger<SerilogLogger>();
    });

    akkaBuilder.WithActors((system, registry, resolver) =>
    {
        var systemProps = resolver.Props<SystemManager>();
        var systemActor = system.ActorOf(systemProps, nameof(SystemManager));
        registry.Register<SystemManager>(systemActor);

        var cropProps = resolver.Props<CropManager>();
        var cropActor = system.ActorOf(cropProps, nameof(CropManager));
        registry.Register<CropManager>(cropActor);
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();
app.UseAgrigateLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();