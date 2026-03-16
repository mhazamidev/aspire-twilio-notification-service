using MediatR;
using Notification.Application.Services;
using Notification.Domain.MessageLogs.Entities;
using Notification.Domain.MessageLogs.Enums;
using Notification.Domain.MessageLogs.Interfaces;
using Notification.Domain.MessageLogs.ValueObjects;
using Notification.Infrastructure.Persistence.Interfaces;
using Notification.Infrastructure.Persistence.UOW;

namespace Notification.Application.Features.SentOtp;

public class SendOtpCommandHandler(
    IEnumerable<INotificationChannel> _channels,
    INotificationUnitofWork _unitofWork,
    OtpGenerator _generator) : IRequestHandler<SendOtpCommand>
{
    public async Task Handle(SendOtpCommand request, CancellationToken cancellationToken)
    {
        var code = _generator.Generate();

        var message = NotificationMessage.Create(
            request.Recipient,
            $"Your verification code is {code}",
            MessageChannel.Otp);

        var channel = _channels
          .First(x => x.Channel == message.Channel);       

        await _unitofWork.Notifications.AddAsync(message);

        await channel.SendAsync(message);
    }
}
