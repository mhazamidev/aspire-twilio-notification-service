using Notification.Worker.Factories;
using Notification.Worker.Handler;
using Notification.Worker.Interfaces;
using Notification.Worker.Processors;
using Notification.Worker.Workers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<NotificationWorker>();

var connection = builder.Configuration.GetSection("ConnectionStrings:rabbitmq").Value;
builder.AddRabbitMQClient(connectionName: connection);

builder.Services.AddScoped<NotificationProcessor>();
builder.Services.AddScoped<INotificationHandler, EmailNotificationHandler>();
builder.Services.AddScoped<INotificationHandler, SmsNotificationHandler>();
builder.Services.AddScoped<INotificationHandler, OtpNotificationHandler>();

builder.Services.AddScoped<INotificationHandlerFactory, NotificationHandlerFactory>();

var host = builder.Build();
host.Run();
