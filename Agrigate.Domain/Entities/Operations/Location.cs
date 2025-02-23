using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Agrigate.Domain.Entities.Common;
using Agrigate.Domain.Enums.Operations;

namespace Agrigate.Domain.Entities.Operations;

/// <summary>
/// A location where business is conducted in some fashion
/// </summary>
[Table(nameof(Location))]
public class Location : EntityBase
{
    public LocationType Type { get; set; }
    
    public long? AddressId { get; set; }
    [ForeignKey(nameof(AddressId))]
    public Address? Address { get; set; }
    
    /// <summary>
    /// The parent location - required when Type = Site
    /// </summary>
    public long? ParentId { get; set; }
    [ForeignKey(nameof(ParentId))]
    public Location? Parent { get; set; }

    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// TODO: Whenever a Consumable or Equipment record is created, create a supplier location and set Metadata to hide that location from regular farm interactions
    /// TODO: The created location should be used for the first ItemTransfer record
    /// TODO: Whenever a Product record is created, the first ItemTransfer record should be from the location of the crop
    /// </summary>
    public ICollection<LocationMetadata> Metadata { get; set; } = [];
}