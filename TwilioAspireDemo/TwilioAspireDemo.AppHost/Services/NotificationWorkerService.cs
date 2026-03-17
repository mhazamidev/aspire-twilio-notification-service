namespace TwilioAspireDemo.AppHost.Services;

public static class NotificationWorkerService
{
    public static void AddNotificationWorkerService(
       this IDistributedApplicationBuilder builder,
       IResourceBuilder<RabbitMQServerResource> rabbitMQ)
    {
        builder.AddProject<Projects.Notification_Worker>("notification-worker")
            .WithReference(rabbitMQ);
    }
}
