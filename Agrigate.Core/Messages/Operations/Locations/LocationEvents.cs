namespace Agrigate.Core.Messages.Operations.Locations;

public static class LocationEvents
{
    public sealed record CreateLocationFailed(LocationName Name, string Reason);
}