namespace BuildingBlocks.Messaging;

public static class RoutingKeys
{
    public const string Email = "notification.email.send";
    public const string Sms = "notification.sms.send";
    public const string Otp = "notification.otp.send";
    public const string Dlq = "notification.Dlq.send";
}
