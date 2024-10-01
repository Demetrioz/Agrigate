namespace Agrigate.Domain.Entities.Rules;

/// <summary>
/// The definition of an action related to notifications
/// </summary>
public class NotificationDefinition
{
    public NotificationChannel Channel { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}