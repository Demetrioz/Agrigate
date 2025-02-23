namespace Agrigate.Domain.Enums.Operations;

public enum LocationType
{
    /// <summary>
    /// A location with a physical address
    /// </summary>
    Operating = 0,
    
    /// <summary>
    /// A type of location that exists within an operating location 
    /// </summary>
    Site = 1
}