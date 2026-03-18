using Notification.Worker;
using Notification.Worker.Processors;
using Notification.Worker.Workers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<NotificationWorker>();
var connection = builder.Configuration.GetSection("ConnectionStrings:rabbitmq").Value;
builder.AddRabbitMQClient(connectionName: connection);
builder.Services.AddScoped<NotificationProcessor>();

var host = builder.Build();
host.Run();
