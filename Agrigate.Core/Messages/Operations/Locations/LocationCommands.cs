namespace Agrigate.Core.Messages.Operations.Locations;

public static class LocationCommands
{
    public sealed record CreateLocation(
        LocationName Name,
        LocationId? Id,
        OrderedParentIds? ParentIds
    );
    
    public sealed record SetChildId(LocationName Name, LocationId Id);
}