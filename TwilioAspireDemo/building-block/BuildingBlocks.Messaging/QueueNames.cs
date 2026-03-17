namespace BuildingBlocks.Messaging;


public static class QueueNames
{
    public const string Email = "email_queue";
    public const string Sms = "sms_queue";
    public const string Otp = "otp_queue";

    public const string Retry = "notification_retry_queue";
    public const string Dlq = "notification_dlq";
}
