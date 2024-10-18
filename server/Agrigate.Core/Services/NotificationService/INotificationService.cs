using Agrigate.Core.Services.NotificationService.Models;
using Agrigate.Domain.Entities;

namespace Agrigate.Core.Services.NotificationService;

/// <summary>
/// Service to send notifications to a user
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Send a notification via MQTT
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SendMqttNotification(
        string topic, 
        string message,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Saves a new notification to the database
    /// </summary>
    /// <param name="title">The title of the notification</param>
    /// <param name="text">The notification text</param>
    /// <param name="timestamp">The timestamp of the notification</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Notification> SaveNotification(
        string title,
        string text,
        DateTimeOffset? timestamp = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Retrieves the 10 most recent notifications
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<NotificationBase>> GetRecentNotifications(
        CancellationToken cancellationToken = default
    );
}