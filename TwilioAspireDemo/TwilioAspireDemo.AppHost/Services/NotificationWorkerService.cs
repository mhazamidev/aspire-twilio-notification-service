namespace TwilioAspireDemo.AppHost.Services;

public static class NotificationWorkerService
{
    public static void AddNotificationWorkerService(
       this IDistributedApplicationBuilder builder,
       IResourceBuilder<SqlServerDatabaseResource> database,
       IResourceBuilder<RabbitMQServerResource> rabbitMQ)
    {
        builder.AddProject<Projects.Notification_Worker>("notification-worker")
            .WithReference(rabbitMQ)
            .WithReference(database);
    }
}
