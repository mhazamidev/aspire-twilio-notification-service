namespace Notification.Infrastructure.Persistence.Interfaces;

public interface INotificationService
{
    Task SendEmailAsync(string to, string content);
    Task SendSmsAsync(string to, string content);
    Task SendOtpAsync(string to, string code);
}
