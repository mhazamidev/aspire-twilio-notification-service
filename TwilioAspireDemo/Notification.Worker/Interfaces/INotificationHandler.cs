using BuildingBlocks.Contracts.DTO;
using Notification.Domain.MessageLogs.Enums;

namespace Notification.Worker.Interfaces;

public interface INotificationHandler
{
    public MessageChannel Channel { get; }
    public Task HandleAsync(NotificationDto dto);
}
