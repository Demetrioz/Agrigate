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
    
    /*
     * TODO: Add a generic Location for Supplier. Adding a new item will create an ItemTransfer with that as the source
     */
    
    /// <summary>
    /// The parent location - required when Type = Site
    /// </summary>
    public long? ParentId { get; set; }
    [ForeignKey(nameof(ParentId))]
    public Location? Parent { get; set; }

    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    public ICollection<LocationMetaData> MetaData { get; set; } = [];
}