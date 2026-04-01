using TwilioAspireDemo.AppHost.Extensions;

namespace TwilioAspireDemo.AppHost.Services;

public static class NotificationService
{
    public static void AddNotificationService(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<SqlServerDatabaseResource> database,
        IResourceBuilder<RabbitMQServerResource> rabbitMQ,
        IResourceBuilder<ParameterResource> accountSid,
        IResourceBuilder<ParameterResource> authToken,
        IResourceBuilder<ParameterResource> fromNumber,
        IResourceBuilder<ParameterResource> sendGridApiKey,
        IResourceBuilder<ParameterResource> verifyServiceSid)
    {
        builder.AddProject<Projects.Notification_Api>("notification-api")
            .WaitFor(database)
            .WithReference(database)
            .WithReference(rabbitMQ)
            .WithTwilio(accountSid, authToken, fromNumber, sendGridApiKey, verifyServiceSid);
    }
}
