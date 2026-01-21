using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Repositories;

public class ParentNotificationRepository(AppDbContext dbContext) : IParentNotificationRepository
{
    public async Task AddParentNotificationAsync(ParentNotification parentNotification)
    {
        await dbContext.ParentNotifications.AddAsync(parentNotification);
    }

    public async Task<Result> RemoveParentNotificationAsync(Guid id)
    {
        var parentNotification = await dbContext.ParentNotifications.FindAsync(id);
        if (parentNotification == null)
            return Result.Failure("Không tìm thấy thông báo phụ huynh");
        dbContext.ParentNotifications.Remove(parentNotification);
        return Result.Success();
    }

    public void UpdateParentNotificationAsync(ParentNotification parentNotification)
    {
        dbContext.ParentNotifications.Update(parentNotification);
    }

    public async Task<ParentNotification?> GetParentNotificationByIdAsync(Guid parentNotificationId)
    {
        return await dbContext.ParentNotifications.FindAsync(parentNotificationId);
    }

    public async Task<IEnumerable<ParentNotification>> GetParentNotificationsByParentAsync(Guid parentId, int pageNumber, int pageSize)
    {
        var query = from notification in dbContext.ParentNotifications
            where notification.ParentId == parentId
            orderby notification.IsRead ascending, notification.CreatedAt descending
            select notification;
        
        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> CountUnreadNotificationsByParentAsync(Guid parentId)
    {
        var query = from notification in dbContext.ParentNotifications
            where notification.ParentId == parentId && !notification.IsRead
            select notification;
        
        return await query.CountAsync();
    }

    public async Task ReadAllNotificationsForParentAsync(Guid parentId)
    {
        var query = from notification in dbContext.ParentNotifications
            where notification.ParentId == parentId && !notification.IsRead
            select notification;
        
        var unreadNotifications = await query.ToListAsync();
        foreach (var notification in unreadNotifications)
            notification.IsRead = true;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync();
    }
}