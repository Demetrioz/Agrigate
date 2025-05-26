using System.ComponentModel.DataAnnotations;

namespace Agrigate.Domain.Entities.Common;

/// <summary>
/// Common properties that belong to all Agrigate entities
/// </summary>
public abstract class EntityBase
{
    /// <summary>
    /// The primary key
    /// </summary>
    [Key]
    public long Id { get; set; }
    
    /// <summary>
    /// The date the record was created
    /// </summary>
    public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;
    
    /// <summary>
    /// The most recent date the record was modified
    /// </summary>
    public DateTimeOffset Modified { get; set; } = DateTimeOffset.Now;
    
    /// <summary>
    /// A soft-delete
    /// </summary>
    public bool IsDeleted { get; set; }
}