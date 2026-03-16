using MediatR;
using Notification.Domain.MessageLogs.Entities;
using Notification.Domain.MessageLogs.Enums;
using Notification.Domain.MessageLogs.Interfaces;
using Notification.Infrastructure.Persistence.UOW;

namespace Notification.Application.Features.Whatsapp;

public class SendWhatsappCommandHandler(
    IEnumerable<INotificationChannel> _channels,
    INotificationUnitofWork _unitofWork) : IRequestHandler<SendWhatsappCommand, bool>
{
    public async Task<bool> Handle(SendWhatsappCommand request, CancellationToken cancellationToken)
    {
        var message = NotificationMessage.Create(
            request.Phone,
            request.Message,
            MessageChannel.WhatsApp);

        var channel = _channels.First(c => c.Channel == message.Channel);

        await channel.SendAsync(message);

        return true;
    }
}
