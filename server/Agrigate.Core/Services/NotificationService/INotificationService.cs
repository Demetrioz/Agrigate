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
}