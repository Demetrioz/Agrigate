namespace Agrigate.Domain.Configuration;

/// <summary>
/// Configuration for connecting to the Agrigate database
/// </summary>
public class DatabaseOptions
{
    /// <summary>
    /// The database hostname
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// The database port 
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// The database name
    /// </summary>
    public string Database { get; set; } = string.Empty;

    /// <summary>
    /// The user to connect with
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// The password to connect with
    /// </summary>
    public string Password { get; set; } = string.Empty;
}