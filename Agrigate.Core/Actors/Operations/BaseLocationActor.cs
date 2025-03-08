using Agrigate.Core.Actors.Operations;
using Agrigate.Core.Messages.Operations.Locations;
using Akka.DependencyInjection;

namespace Agrigate.Core.Actors.Operations;

/// <summary>
/// Base functionality shared by both the LocationManager and LocationActor
/// </summary>
public abstract class BaseLocationActor : UntypedActor
{
    protected Dictionary<long, string> Children { get; set; } = new();

    protected override void PreStart()
    {
        // TODO: Load all child locations and create appropriate actors -- does persistence help with any of this?
    }
    
    protected void CreateChildLocation(LocationCommands.CreateLocation command)
    {
        var childName = $"location-{command.Name}";
        if (command.ParentIds != null)
        {
            var topMostParent = command.ParentIds.Value.Dequeue();
            if (!Children.TryGetValue(topMostParent, out childName))
                Sender.Tell(new LocationEvents.CreateLocationFailed(command.Name, "Invalid ParentId"));
        }
        
        var child = Context.Child(childName);
        if (child.IsNobody())
        {
            var props = DependencyResolver.For(Context.System).Props<LocationActor>(childName, command.Id?.Value);
            child = Context.ActorOf(props, childName);
        }
        
        if (command.Id.HasValue)
            AddChildMapping(new LocationCommands.SetChildId(command.Name, command.Id.Value));
        
        child.Forward(command);
    }

    protected void AddChildMapping(LocationCommands.SetChildId command)
    {
        if (!Children.TryGetValue(command.Id.Value, out _))
            Children.Add(command.Id.Value, command.Name.Value);
    }
}