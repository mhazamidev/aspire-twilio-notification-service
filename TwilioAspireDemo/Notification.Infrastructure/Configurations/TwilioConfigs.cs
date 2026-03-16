namespace Notification.Infrastructure.Configurations;

public class TwilioConfigs
{
    public string AccountSid { get; set; } = string.Empty;
    public string AuthToken { get; set; } = string.Empty;
    public string FromNumber { get; set; } = string.Empty;
    public string SendGridApiKey { get; set; } = string.Empty;
    public string VerifyServiceSid { get; set; } = string.Empty;
}
