using AutoMapper;
using eBoardAPI.Common;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.Notification;

namespace eBoardAPI.Services;

public class ParentNotificationService(
    IParentNotificationRepository parentNotificationRepository,
    IMapper mapper
    ) : IParentNotificationService
{
    public async Task SendNotificationToParentAsync(Guid parentId, string message)
    {
        var notification = new Entities.ParentNotification
        {
            Id = Guid.NewGuid(),
            ParentId = parentId,
            Message = message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };
        await parentNotificationRepository.AddParentNotificationAsync(notification);
        await parentNotificationRepository.SaveChangesAsync();
    }

    public async Task<Result> RemoveNotificationAsync(Guid notificationId)
    {
        return await parentNotificationRepository.RemoveParentNotificationAsync(notificationId);
    }

    public async Task ReadNotificationAsync(Guid notificationId)
    {
        var notification = await parentNotificationRepository.GetParentNotificationByIdAsync(notificationId);
        if (notification != null)
        {
            notification.IsRead = true;
            await parentNotificationRepository.SaveChangesAsync();
        }
    }

    public async Task ReadAllNotificationsForParentAsync(Guid parentId)
    {
        await parentNotificationRepository.ReadAllNotificationsForParentAsync(parentId);
        await parentNotificationRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<ParentNotificationDto>> GetNotificationsForParentAsync(Guid parentId, int pageNumber,
        int pageSize)
    {
        var notifications =
            await parentNotificationRepository.GetParentNotificationsByParentAsync(parentId, pageNumber, pageSize);
        return mapper.Map<IEnumerable<ParentNotificationDto>>(notifications);
    }
}