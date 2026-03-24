using BuildingBlocks.Contracts.Contracts.Notification;
using MediatR;
using Notification.Application.Features.Sms;
using Notification.Domain.MessageLogs.Enums;
using Notification.Worker.Interfaces;

namespace Notification.Worker.Handler;

public class SmsNotificationHandler : INotificationHandler
{
    private readonly ISender _sender;

    public MessageChannel Channel => MessageChannel.Sms;

    public SmsNotificationHandler(ISender sender)
    {
        _sender = sender;
    }

    public async Task HandleAsync(NotificationDto dto)
    {
        await _sender.Send(new SendSMSCommand(dto.Recipient, dto.Content));
    }
}
