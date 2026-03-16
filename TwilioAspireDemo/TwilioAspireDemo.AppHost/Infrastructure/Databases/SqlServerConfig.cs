namespace TwilioAspireDemo.AppHost.Infrastructure.Databases;

public static class SqlServerConfig
{
    public static IResourceBuilder<SqlServerDatabaseResource> AddIdentityDatabase(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<ParameterResource> password)
    {

        return builder
            .AddSqlServer("sql", password: password)
            .WithHostPort(1435)
            .WithDataVolume()
            .AddDatabase("sqlconnection", "notification_service");
    }
}
