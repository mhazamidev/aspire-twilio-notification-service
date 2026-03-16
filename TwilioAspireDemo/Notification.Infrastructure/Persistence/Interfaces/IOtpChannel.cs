using Notification.Domain.MessageLogs.Interfaces;

namespace Notification.Infrastructure.Persistence.Interfaces;

public interface IOtpChannel: INotificationChannel
{
    Task<bool> VerifyAsync(string phoneNumber, string code);
}
