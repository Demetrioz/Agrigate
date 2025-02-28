using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agrigate.Domain.Entities.System;

[Table("Settings")]
public class Setting : EntityBase
{
    [MaxLength(255)]
    public string Key { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string? Value { get; set; }
}