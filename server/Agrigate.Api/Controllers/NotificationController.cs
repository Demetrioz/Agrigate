using Agrigate.Api.Core;
using Agrigate.Core.Services.NotificationService;
using Microsoft.AspNetCore.Mvc;

namespace Agrigate.Api.Controllers;

/// <summary>
/// Controller for handling notification-related requests
/// </summary>
[Route("Notifications")]
public class NotificationController : AgrigateController
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService) 
    {
        _notificationService = notificationService 
            ?? throw new ArgumentNullException(nameof(notificationService));
    }

    /// <summary>
    /// Retrieves the most recent notifications
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<object> GetNotifications(
        CancellationToken cancellationToken = default
    )
    {
        try 
        {
            var result = await _notificationService
                .GetRecentNotifications(cancellationToken);

            return Success(result);
        }
        catch (Exception ex)
        {
            return Failure(ex.Message);
        }
    }
}