namespace Agrigate.Api.Models.Requests;

/// <summary>
/// Request to mark a list of notifications as "has been read"
/// </summary>
/// <param name="Ids"></param>
public record NotificationsRead(List<long> Ids);