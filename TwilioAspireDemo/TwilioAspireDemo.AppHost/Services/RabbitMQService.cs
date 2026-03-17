namespace TwilioAspireDemo.AppHost.Services;

public static class RabbitMQService
{
    public static IResourceBuilder<RabbitMQServerResource> AddRabbitMQService(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<ParameterResource> username,
        IResourceBuilder<ParameterResource> passworde)
    {
        return builder.AddRabbitMQ("rabbitmq", userName: username, password: passworde)
              .WithManagementPlugin()
              .WithDataVolume(isReadOnly: false);

    }
}
