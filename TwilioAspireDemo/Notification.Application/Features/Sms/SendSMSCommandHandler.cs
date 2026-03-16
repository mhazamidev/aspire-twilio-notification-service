using MediatR;
using Notification.Domain.MessageLogs.Entities;
using Notification.Domain.MessageLogs.Enums;
using Notification.Domain.MessageLogs.Interfaces;
using Notification.Infrastructure.Persistence.UOW;

namespace Notification.Application.Features.Sms;

public class SendSMSCommandHandler(
    IEnumerable<INotificationChannel> _channels,
    INotificationUnitofWork _unitofWork) : IRequestHandler<SendSMSCommand, bool>
{
    public async Task<bool> Handle(SendSMSCommand request, CancellationToken cancellationToken)
    {
        var message = NotificationMessage.Create(
            request.Phone,
            request.Message,
            MessageChannel.Sms);

        var channel = _channels.First(x => x.Channel == message.Channel);

        await channel.SendAsync(message);

        return true;
    }
}
