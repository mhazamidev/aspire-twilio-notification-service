using MediatR;
using Notification.Domain.MessageLogs.Entities;
using Notification.Domain.MessageLogs.Enums;
using Notification.Domain.MessageLogs.Interfaces;
using Notification.Infrastructure.Persistence.UOW;

namespace Notification.Application.Features.Voice;

public class SendVoiceCommandHandler(
    IEnumerable<INotificationChannel> _channels,
    INotificationUnitofWork _unitofWork) : IRequestHandler<SendVoiceCommand, bool>
{
    public async Task<bool> Handle(SendVoiceCommand request, CancellationToken cancellationToken)
    {
        var message = NotificationMessage.Create(
            request.phone,
            "http://demo.twilio.com/docs/voice.xml",
            MessageChannel.Voice);

        var channel = _channels.First(c => c.Channel == message.Channel);

        await channel.SendAsync(message);

        return true;
    }
}
