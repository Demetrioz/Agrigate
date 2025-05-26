namespace Agrigate.Core.Configuration;

/// <summary>
/// Configuration settings related to logging
/// </summary>
public class LoggingConfiguration
{
    /// <summary>
    /// The host name of the Grafana Loki instance used for capturing logs
    /// </summary>
    public string? LokiHost { get; set; }
}