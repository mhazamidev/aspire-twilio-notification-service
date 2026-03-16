using Domain.SeedWork;
using Notification.Domain.MessageLogs.Interfaces;

namespace Notification.Infrastructure.Persistence.UOW;

public interface INotificationUnitofWork : IUnitOfWork
{
    public INotificationRepository Notifications { get; }
}
