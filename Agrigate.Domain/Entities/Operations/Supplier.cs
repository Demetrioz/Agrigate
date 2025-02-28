using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Agrigate.Domain.Entities.Common;

namespace Agrigate.Domain.Entities.Operations;

[Table(nameof(Supplier))]
public class Supplier : EntityBase
{
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    
    public long? AddressId { get; set; }
    [ForeignKey(nameof(AddressId))]
    public Address? Address { get; set; }
}