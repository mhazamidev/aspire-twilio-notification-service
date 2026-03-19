using BuildingBlocks.Contracts.DTO;
using MediatR;
using Notification.Application.Features.Email;
using Notification.Domain.MessageLogs.Enums;
using Notification.Worker.Interfaces;

namespace Notification.Worker.Handler;

public class EmailNotificationHandler : INotificationHandler
{
    private readonly ISender _sender;

    public EmailNotificationHandler(ISender sender)
    {
        _sender = sender;
    }

    public MessageChannel Channel => MessageChannel.Email;

    public async Task HandleAsync(NotificationDto dto)
    {
        await _sender.Send(new SendEmailCommand(dto.Recipient, dto.Content));
    }
}