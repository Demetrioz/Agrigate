using Serilog;
using Serilog.Events;
using Serilog.Sinks.Grafana.Loki;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
{
    config.MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning);
    config.MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning);
    config.MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning);
    
    config.WriteTo.Console();

    #if !DEBUG
    config.WriteTo.GrafanaLoki(
        "http://loki:3100",
        labels: [
            new LokiLabel
            {
                Key = "source",
                Value = "Agrigate.Api"
            }
        ],
        propertiesAsLabels: [
            "source"
        ]
    );
    #endif
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();