using System.Reflection;
using Agrigate.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Grafana.Loki;

namespace Agrigate.Core.Extensions;

/// <summary>
/// Extensions for setting up logging
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// Configures logging using Serilog with Console and Loki sinks. Requires the Logging__LokiHost configuration
    /// value in order to enable logging to Grafana Loki
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IHostBuilder ConfigureAgrigateLogging(this IHostBuilder builder, IConfiguration configuration)
    {
        var settings = new LoggingConfiguration();
        configuration.Bind(Constants.Logging.Configuration, settings);

        var sourceProject = Assembly
            .GetEntryAssembly()?.EntryPoint?.DeclaringType?.Assembly?.FullName?
            .Split(",")
            .FirstOrDefault() 
            ?? Constants.Logging.DefaultSource;
        
        builder.UseSerilog((context, config) =>
        {
            // Remove the noisy .net core logs
            config.MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning);
            config.MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning);
            config.MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning);
            config.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning);
            
            config.WriteTo.Console(outputTemplate: "[{Level:u3}][{Timestamp:yyyy-MM-dd HH:mm:ss}][{Thread}][{ActorPath}] {Message:lj}{NewLine}{Exception}");

            if (!string.IsNullOrWhiteSpace(settings.LokiHost))
                config.WriteTo.GrafanaLoki(
                    settings.LokiHost,
                    labels: [
                        new LokiLabel
                        {
                            Key = Constants.Logging.Labels.ServiceName,
                            Value = sourceProject
                        }
                    ],
                    propertiesAsLabels: [
                        Constants.Logging.Labels.ServiceName,
                    ]
                );
        });
        
        return builder;
    }

    /// <summary>
    /// A wrapper for UseSerilogRequestLogging to clean up Host logs on the API
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseAgrigateLogging(this IApplicationBuilder builder)
    {
        return builder.UseSerilogRequestLogging();
    }
}