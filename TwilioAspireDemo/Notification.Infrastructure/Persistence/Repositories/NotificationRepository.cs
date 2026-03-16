using Microsoft.EntityFrameworkCore;
using Notification.Domain.MessageLogs.Entities;
using Notification.Domain.MessageLogs.Interfaces;
using Notification.Domain.MessageLogs.ValueObjects;
using Notification.Infrastructure.Persistence.Database;

namespace Notification.Infrastructure.Persistence.Repositories;

public class NotificationRepository(NotificationDbContext _dbContext) : INotificationRepository
{
    public async Task AddAsync(NotificationMessage message)
    {
        await _dbContext.Notifications.AddAsync(message);
    }

    public async Task<NotificationMessage?> GetAsync(NotificationMessageId id)
        => await _dbContext.Notifications.FirstOrDefaultAsync(x => x.Id == id);

}
