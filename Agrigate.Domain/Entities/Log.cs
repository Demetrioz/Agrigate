using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Logging;

namespace Agrigate.Domain.Entities;

[Table(nameof(Log))]
public class Log
{
    [Key]
    public long Id { get; set; }
    public DateTime Timestamp { get; set; }
    public LogLevel LogLevel { get; set; }
    
    [MaxLength(1024)]
    public string Message { get; set; } = string.Empty;
    
    [MaxLength(512)]
    public string? Source { get; set; }
    
    [MaxLength(2048)]
    public string? Data { get; set; }
    
    [MaxLength(5120)]
    public string? StackTrace { get; set; }
}