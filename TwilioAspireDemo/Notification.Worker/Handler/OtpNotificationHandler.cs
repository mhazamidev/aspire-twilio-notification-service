using BuildingBlocks.Contracts.DTO;
using MediatR;
using Notification.Application.Features.SentOtp;
using Notification.Domain.MessageLogs.Enums;
using Notification.Worker.Interfaces;

namespace Notification.Worker.Handler;

public class OtpNotificationHandler : INotificationHandler
{
    private readonly ISender _sender;

    public MessageChannel Channel => MessageChannel.Otp;

    public OtpNotificationHandler(ISender sender)
    {
        _sender = sender;
    }

    public async Task HandleAsync(NotificationDto dto)
    {
        await _sender.Send(new SendOtpCommand(dto.Recipient));
    }
}
