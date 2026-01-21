using eBoardAPI.Common;
using eBoardAPI.Entities;

namespace eBoardAPI.Interfaces.Repositories;

public interface IParentNotificationRepository
{
    Task AddParentNotificationAsync(ParentNotification parentNotification);
    Task<Result> RemoveParentNotificationAsync(Guid id);
    void UpdateParentNotificationAsync(ParentNotification parentNotification);
    Task<ParentNotification?> GetParentNotificationByIdAsync(Guid parentNotificationId);
    Task<IEnumerable<ParentNotification>> GetParentNotificationsByParentAsync(Guid parentId, int pageNumber, int pageSize);
    Task<int> CountUnreadNotificationsByParentAsync(Guid parentId);
    Task ReadAllNotificationsForParentAsync(Guid parentId);
    Task<int> SaveChangesAsync();
}