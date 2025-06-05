using Agrigate.Domain.Contexts;
using Akka.Actor;
using Akka.Event;
using Akka.Logger.Serilog;
using Microsoft.EntityFrameworkCore;

namespace Agrigate.Api.System.Actors;

/// <summary>
/// The root actor for overall system management
/// </summary>
public class SystemManager : ReceiveActor
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILoggingAdapter _logger;

    /// <summary>
    /// The constructor
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public SystemManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = Context.GetLogger<SerilogLoggingAdapter>();
    }

    protected override void PreStart()
    {
        _logger.Info("Initializing Agrigate...");
        
        RunMigrations();
        
        _logger.Info("Initialization completed!");
    }

    private void RunMigrations()
    {
        _logger.Info("Running database migrations...");
        
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AgrigateContext>();
        db.Database.Migrate();
        
        _logger.Info("Database migrations completed!");
    }
}