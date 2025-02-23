using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agrigate.Domain.Entities.Operations;

/// <summary>
/// Represents equipment that has been purchased
/// </summary>
[Table(nameof(Equipment))]
public class Equipment : ItemBase
{
    [MaxLength(128)]
    public string? Make { get; set; }
    
    [MaxLength(128)]
    public string? Model { get; set; }
    
    [MaxLength(128)]
    public string? SerialNumber { get; set; }
}