using System.ComponentModel.DataAnnotations.Schema;

namespace Agrigate.Domain.Entities.Production;

/// <summary>
/// Represents an instance of a particular type of produce being planted
/// </summary>
[Table(nameof(Crop))]
public class Crop : EntityBase
{
    
}