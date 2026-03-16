using MediatR;
using Notification.Domain.MessageLogs.Entities;
using Notification.Domain.MessageLogs.Enums;
using Notification.Domain.MessageLogs.Interfaces;
using Notification.Infrastructure.Persistence.UOW;

namespace Notification.Application.Features.Email;

public class SendEmailCommandHandler(
    IEnumerable<INotificationChannel> _channels,
    INotificationUnitofWork _unitofWork) : IRequestHandler<SendEmailCommand, bool>
{
    public async Task<bool> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        var message = NotificationMessage.Create(
            request.Email,
            request.Message,
            MessageChannel.Email);

        var channel = _channels.First(c => c.Channel == message.Channel);

        await channel.SendAsync(message);

        return true;
    }
}
