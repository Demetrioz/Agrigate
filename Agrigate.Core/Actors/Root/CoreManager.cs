using Agrigate.Domain.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Agrigate.Core.Actors.Root;

/// <summary>
/// Top-level actor that manages the overall Agrigate.Core actor system
/// </summary>
public class CoreManager : ReceiveActor
{
    private readonly IServiceProvider _serviceProvider;
    
    public CoreManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override void PreStart()
    {
        using var scope = _serviceProvider.CreateScope();
        scope.ServiceProvider.ApplyAgrigateMigrations();
    }
}