namespace Agrigate.Core.Services.NotificationService.Models;

/// <summary>
/// Minimum information for a notification
/// </summary>
public class NotificationBase
{
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public bool HasBeenViewed { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}