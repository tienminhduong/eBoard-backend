using eBoardAPI.Common;
using eBoardAPI.Models.Notification;

namespace eBoardAPI.Interfaces.Services;

public interface IParentNotificationService
{
    Task SendNotificationToParentAsync(Guid parentId, string message);
    Task<Result> RemoveNotificationAsync(Guid notificationId);
    Task ReadNotificationAsync(Guid notificationId);
    Task ReadAllNotificationsForParentAsync(Guid parentId);
    Task<IEnumerable<ParentNotificationDto>> GetNotificationsForParentAsync(Guid parentId, int pageNumber, int pageSize);
}