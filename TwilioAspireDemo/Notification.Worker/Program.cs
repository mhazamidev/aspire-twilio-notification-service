using Notification.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
var connection = builder.Configuration.GetSection("ConnectionStrings:rabbitmq").Value;
builder.AddRabbitMQClient(connectionName: connection);

var host = builder.Build();
host.Run();
