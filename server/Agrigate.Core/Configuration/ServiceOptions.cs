namespace Agrigate.Core.Configuration;

/// <summary>
/// Configuration for services using Akka.Remote
/// </summary>
public class ServiceOptions
{
    /// <summary>
    /// The serice name used by Akka.Remote
    /// </summary>
    public string ServiceName { get; set; } = string.Empty;

    /// <summary>
    /// The root actor of the service
    /// </summary>
    public string SupervisorName { get; set; } = string.Empty;

    /// <summary>
    /// The public host name off the service used by Akka.Remote
    /// </summary>
    public string Hostname { get; set; } = string.Empty;

    /// <summary>
    /// The port used by Akka.Remote
    /// </summary>
    public int Port { get; set; }
}