using BuildingBlocks.Contracts.Contracts.Notification;
using BuildingBlocks.Messaging;
using Notification.Infrastructure.Persistence.Interfaces;

namespace Notification.Application.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationPublisher _publisher;

    public NotificationService(INotificationPublisher publisher)
    {
        _publisher = publisher;
    }
    public async Task SendEmailAsync(string to, string content)
    {
        var dto = new NotificationDto
        {
            Recipient = to,
            Content = content,
            Channel = "email"
        };
        await _publisher.PublishAsync(dto, RoutingKeys.Email);
    }

    public async Task SendOtpAsync(string to, string code)
    {
        var dto = new NotificationDto
        {
            Recipient = to,
            Content = code,
            Channel = "otp"
        };
        await _publisher.PublishAsync(dto, RoutingKeys.Otp);
    }

    public async Task SendSmsAsync(string to, string content)
    {
        var dto = new NotificationDto
        {
            Recipient = to,
            Content = content,
            Channel = "sms"
        };
        await _publisher.PublishAsync(dto, RoutingKeys.Sms);
    }
}
