using Notification.Application.Messaging;
using Notification.Infrastructure.Persistence.Interfaces;
using Notification.Worker.Factories;
using Notification.Worker.Handler;
using Notification.Worker.Interfaces;
using Notification.Worker.Processors;
using Notification.Worker.Workers;
using Notofication.Infrastructure.IoC.DI;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<NotificationWorker>();

builder.AddRabbitMQ();


builder.Services.AddScoped<NotificationProcessor>();
builder.Services.AddScoped<INotificationHandler, EmailNotificationHandler>();
builder.Services.AddScoped<INotificationHandler, SmsNotificationHandler>();
builder.Services.AddScoped<INotificationHandler, OtpNotificationHandler>();

builder.Services.AddScoped<INotificationHandlerFactory, NotificationHandlerFactory>();
builder.Services.AddScoped<IRetryHandler, RetryHandler>();

builder.Services.AddTwilio();
builder.Services.AddTwilioConfig(builder.Configuration);
builder.Services.AddRepositories();

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddMediatRSetup();
builder.Services.AddServices();

var host = builder.Build();
host.Run();
