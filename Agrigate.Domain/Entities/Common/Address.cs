using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agrigate.Domain.Entities.Common;

[Table(nameof(Address))]
public class Address : EntityBase
{
    [MaxLength(255)]
    public string Line1 { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string? Line2 { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string City { get; set; } = string.Empty;
    
    [MaxLength(16)]
    public string State { get; set; } = string.Empty;
    
    [MaxLength(10)]
    public string PostalCode { get; set; } = string.Empty;
}