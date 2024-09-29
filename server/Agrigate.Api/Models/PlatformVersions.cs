namespace Agrigate.Api.Models;

/// <summary>
/// Model containing the version numbers for all Agrigate Platform entities
/// </summary>
/// <param name="Api"></param>
/// <param name="EventService"></param>
public class PlatformVersions
{
    public string Api { get; set; } = "0.0.0";
    public string EventService { get; set; } = "0.0.0";
}
