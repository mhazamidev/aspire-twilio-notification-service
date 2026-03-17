using Microsoft.Extensions.Logging;
using Notification.Domain.MessageLogs.Interfaces;
using Notification.Infrastructure.Persistence.Database;
using Notification.Infrastructure.Persistence.Repositories;
using Notification.Infrastructure.Persistence.UOW;

namespace Notification.Application.UoW;

public class NotificationUnitofWork : UnitOfWork, INotificationUnitofWork
{
    private readonly NotificationDbContext _dbContext;
    public NotificationUnitofWork(
        NotificationDbContext dbContext,
        ILogger<NotificationDbContext> logger) : base(dbContext, logger)
    {
        _dbContext = dbContext;
    }

    public INotificationRepository Notifications => new NotificationRepository(_dbContext);
}
