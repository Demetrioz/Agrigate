using Agrigate.Core.Messages.Operations.Locations;
using Agrigate.Domain.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Agrigate.Core.Actors.Operations;

/// <summary>
/// Manages all locations within Agrigate
/// </summary>
public class LocationManager : BaseLocationActor
{
    private readonly IServiceProvider _serviceProvider;

    public LocationManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    protected override void PreStart()
    {
        LoadRootLocations();
    }

    private void LoadRootLocations()
    {
        using var scope = _serviceProvider.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<AgrigateDbContext>();
        
        var rootLocations = dbContext.Locations
            .AsNoTracking()
            .Where(l =>
                !l.IsDeleted
                && l.ParentId == null
            )
            .ToList();

        foreach (var location in rootLocations)
        {
            var name = new LocationName(location.Name);
            var id = new LocationId(location.Id);
            
            CreateChildLocation(new LocationCommands.CreateLocation(name, id, null));
        }
    }

    protected override void OnReceive(object message)
    {
        switch (message)
        {
            case LocationCommands.CreateLocation create:
                CreateChildLocation(create);
                break;
            
            case LocationCommands.SetChildId update:
                AddChildMapping(update);
                break;
            
            default:
                Unhandled(message);
                break;
        }
    }
}