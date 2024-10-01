namespace Agrigate.Core.Configuration;

/// <summary>
/// Configuration for sending notifications
/// </summary>
public class NotificationOptions
{
    /// <summary>
    /// The clientId used when connecting to the MQTT broker
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// The MQTT host
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// The MQTT port
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// The username for connecting to the MQTT broker
    /// </summary>
    public string Username { get; set; } = string.Empty;
    
    /// <summary>
    /// The password for connecting to the MQTT broker
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Whether TLS should be used when connecting to MQTT
    /// </summary>
    public bool SecureConnection { get; set; }
}