namespace Agrigate.EventService.Configuration;

/// <summary>
/// Configuration for connecting to an MQTT Broker to retrieve telemetry 
/// readings
/// </summary>
public class TelemetryOptions
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
    /// The topic subscribed to for retrieving telemetry
    /// </summary>
    public string Topic { get; set; } = string.Empty;

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