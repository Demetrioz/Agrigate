using System.Reflection;
using Agrigate.Domain.Contexts;
using Akka.Logger.Serilog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Agrigate.Engine.System.Actors;

/// <summary>
/// The root actor for overall system management
/// </summary>
public class SystemManager : ReceiveActor
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILoggingAdapter _logger;

    private readonly string _engineVersion;

    /// <summary>
    /// The constructor
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public SystemManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = Context.GetLogger<SerilogLoggingAdapter>();
        
        _engineVersion = Assembly.GetEntryAssembly()
            ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion ?? "0.0.0";
    }

    /// <summary>
    /// Method that runs at the beginning of the actor lifecycle
    /// </summary>
    protected override void PreStart()
    {
        _logger.Info("Initializing Agrigate system...");
        _logger.Info("Engine version: {0}", _engineVersion);
        
        RunMigrations();
        InitializeRootActors();
        
        _logger.Info("System initialization complete!");
    }

    /// <summary>
    /// Applies any missing migrations to the database
    /// </summary>
    private void RunMigrations()
    {
        _logger.Info("Running database migrations...");
        
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AgrigateContext>();
        db.Database.Migrate();
        
        _logger.Info("Database migrations completed!");
    }

    /// <summary>
    /// Initializes additional root actors
    /// </summary>
    private void InitializeRootActors()
    {
        _logger.Info("Initializing root actors...");
        
        // TODO: Create Root Actors
        
        _logger.Info("Root actors initialized!");
    }
}