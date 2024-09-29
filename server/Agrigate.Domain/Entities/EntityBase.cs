namespace Agrigate.Domain.Entities;

/// <summary>
/// Common properties on all Agrigate entities
/// </summary>
public class EntityBase
{
    /// <summary>
    /// When the entity was created
    /// </summary>
    public DateTimeOffset Created { get; set; }

    /// <summary>
    /// The last time the entity was modified
    /// </summary>
    public DateTimeOffset Modified { get; set; }

    /// <summary>
    /// Whether the entity should be considered deleted
    /// </summary>
    public bool IsDeleted { get; set; }
}