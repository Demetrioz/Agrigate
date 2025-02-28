using System.ComponentModel.DataAnnotations;

namespace Agrigate.Domain.Entities;

/// <summary>
/// Common properties across all Agrigate Entities 
/// </summary>
public class EntityBase
{
    [Key]
    public long Id { get; set; }
    
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Modified { get; set; } = DateTime.Now;
    public bool IsDeleted { get; set; }
}