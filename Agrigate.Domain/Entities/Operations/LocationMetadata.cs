using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agrigate.Domain.Entities.Operations;

/// <summary>
/// Additional information about a particular location, stored as key-value pairs
/// </summary>
[Table(nameof(LocationMetadata))]
public class LocationMetadata : EntityBase
{
    public long LocationId { get; set; }
    [ForeignKey(nameof(LocationId))]
    public Location? Location { get; set; }

    [MaxLength(255)]
    public string Key { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string Value { get; set; } = string.Empty;
}