using System.ComponentModel.DataAnnotations.Schema;

namespace Agrigate.Domain.Entities;

/// <summary>
/// Represents a notification that should be displayed in the app
/// </summary>
[Table(nameof(Notification))]
public class Notification : EntityBase
{
    /// <summary>
    /// Primary Key
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// The notification's title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The main content of the notification
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// The timestamp that the notification was received
    /// </summary>
    public DateTimeOffset Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Whether or not the notification has been viewed
    /// </summary>
    public bool HasBeenViewed { get; set; }
}