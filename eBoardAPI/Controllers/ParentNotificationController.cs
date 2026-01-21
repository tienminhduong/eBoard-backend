using eBoardAPI.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace eBoardAPI.Controllers;

[ApiController]
[Route("api/parent-notifications")]
public class ParentNotificationController(IParentNotificationService parentNotificationService) : ControllerBase
{
    [HttpGet("{parentId}")]
    public async Task<ActionResult> GetNotificationsForParent(Guid parentId, int pageNumber = 1, int pageSize = 20)
    {
        var notifications = await parentNotificationService.GetNotificationsForParentAsync(parentId, pageNumber, pageSize);
        return Ok(notifications);
    }

    [HttpPost("{notificationId}/read")]
    public async Task<ActionResult> ReadNotification(Guid notificationId)
    {
        await parentNotificationService.ReadNotificationAsync(notificationId);
        return NoContent();
    }

    [HttpPost("parent/{parentId}/read-all")]
    public async Task<ActionResult> ReadAllNotificationsForParent(Guid parentId)
    {
        await parentNotificationService.ReadAllNotificationsForParentAsync(parentId);
        return NoContent();
    }

    [HttpDelete("{notificationId}")]
    public async Task<ActionResult> RemoveNotification(Guid notificationId)
    {
        var result = await parentNotificationService.RemoveNotificationAsync(notificationId);
        return result.IsSuccess ? NoContent() : BadRequest(result.ErrorMessage);
    }
}