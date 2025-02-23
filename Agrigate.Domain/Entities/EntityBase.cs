using System.ComponentModel.DataAnnotations;

namespace Agrigate.Domain.Entities;

/// <summary>
/// Common properties across all Agrigate entities
/// </summary>
public class EntityBase
{
    [Key]
    public long Id { get; set; }
    
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public bool IsDeleted { get; set; }
}