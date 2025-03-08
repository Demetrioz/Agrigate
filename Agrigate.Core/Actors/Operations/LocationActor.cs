using Agrigate.Core.Messages.Operations.Locations;
using Agrigate.Domain.Contexts;
using Agrigate.Domain.Entities.Operations;
using Agrigate.Domain.Enums.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Agrigate.Core.Actors.Operations;

/// <summary>
/// Represents a single location within Agrigate
/// </summary>
public class LocationActor : BaseLocationActor
{
    private readonly IServiceProvider _serviceProvider;
    
    private LocationName _name;
    private LocationId? _id;

    public LocationActor(IServiceProvider serviceProvider, LocationName name, LocationId? id)
    {
        _serviceProvider = serviceProvider;
        
        _name = name;
        _id = id;
    }

    /// <summary>
    /// Initial startup behavior
    /// </summary>
    /// <param name="message"></param>
    protected override void OnReceive(object message)
    {
        switch (message)
        {
            // TODO: Handle behavior change differently, based on location type 
            case LocationCommands.CreateLocation create:
                InitializeLocation(create);
                Become(Initialized);
                break;
            
            default:
                Unhandled(message);
                break;
        }
    }
    
    private void InitializeLocation(LocationCommands.CreateLocation command)
    {
        if (command.Id.HasValue)
        {
            LoadChildren();
        }
        else
        {
            // TODO: Create new database record
            // TODO: Send a message with the id back to the parent
            // TODO: Send a response back to the sender
        }
    }
    
    private void LoadChildren()
    {
        using var scope = _serviceProvider.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<AgrigateDbContext>();

        var childLocations = dbContext.Locations
            .AsNoTracking()
            .Where(l =>
                !l.IsDeleted
                && l.ParentId == _id!.Value.Value
            )
            .ToList();

        foreach (var child in childLocations)
        {
            var name = new LocationName(child.Name);
            var id = new LocationId(child.Id);
            
            CreateChildLocation(new LocationCommands.CreateLocation(name, id, null));
        }
    }

    private void CreateLocation(
        LocationType type,
        LocationName name, 
        LocationId? parentId
    )
    {
        using var scope = _serviceProvider.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<AgrigateDbContext>();

        var newLocation = new Location
        {
            Type = type,
            Name = name.Value,
            ParentId = parentId?.Value
        };
        
        dbContext.Locations.Add(newLocation);
    }

    /// <summary>
    /// General behavior after the location has been setup 
    /// </summary>
    /// <param name="message"></param>
    private void Initialized(object message)
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