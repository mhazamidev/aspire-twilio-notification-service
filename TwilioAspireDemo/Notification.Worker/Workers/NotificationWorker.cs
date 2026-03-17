using BuildingBlocks.Contracts.Notification.Contracts;
using BuildingBlocks.Messaging;
using BuildingBlocks.Utility;
using MediatR;
using Notification.Application.Features.Email;
using Notification.Application.Features.SentOtp;
using Notification.Application.Features.Sms;
using Notification.Domain.MessageLogs.Enums;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Notification.Worker.Workers;

public class NotificationWorker(
    ILogger<NotificationWorker> logger,
    IConnection connection,
    IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(
            exchange: Exchanges.Notification,
            type: ExchangeType.Topic,
            durable: true);

        await channel.QueueDeclareAsync(QueueNames.Email, true, false, false);
        await channel.QueueDeclareAsync(QueueNames.Sms, true, false, false);
        await channel.QueueDeclareAsync(QueueNames.Otp, true, false, false);

        var retryArgs = new Dictionary<string, object>
        {
            { "x-message-ttl", 10000 },
            { "x-dead-letter-exchange", Exchanges.Notification },
            { "x-dead-letter-routing-key", RoutingKeys.Email } //Can be dynamic based on the original routing key
        };

        await channel.QueueDeclareAsync(
            QueueNames.Retry,
            true,
            false,
            false,
            retryArgs);

        await channel.QueueDeclareAsync(QueueNames.Dlq, true, false, false);

        await channel.QueueBindAsync(QueueNames.Email, Exchanges.Notification, RoutingKeys.Email);
        await channel.QueueBindAsync(QueueNames.Sms, Exchanges.Notification, RoutingKeys.Sms);
        await channel.QueueBindAsync(QueueNames.Otp, Exchanges.Notification, RoutingKeys.Otp);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (sender, eventArgs) =>
        {
            var json = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

            NotificationEnvelope? envelope = null;

            try
            {
                envelope = JsonSerializer.Deserialize<NotificationEnvelope>(json);

                if (envelope == null)
                    throw new Exception("Invalid message");

                using var scope = scopeFactory.CreateScope();
                var senderService = scope.ServiceProvider.GetRequiredService<ISender>();

                var messageChannel = envelope.Payload.Channel.GetEnumValue<MessageChannel>();

                switch (messageChannel)
                {
                    case MessageChannel.Email:
                        await senderService.Send(new SendEmailCommand(envelope.Payload.Recipient, envelope.Payload.Content));
                        break;
                    case MessageChannel.Sms:
                        await senderService.Send(new SendSMSCommand(envelope.Payload.Recipient, envelope.Payload.Content));
                        break;
                    case MessageChannel.Otp:
                        await senderService.Send(new SendOtpCommand(envelope.Payload.Recipient));
                        break;
                }

                await channel.BasicAckAsync(eventArgs.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing message");

                if (envelope != null)
                {
                    envelope.RetryCount++;

                    if (envelope.RetryCount <= 3)
                    {
                        var retryJson = JsonSerializer.Serialize(envelope);
                        var retryBody = Encoding.UTF8.GetBytes(retryJson);

                        await channel.BasicPublishAsync(
                            exchange: Exchanges.Notification,
                            routingKey: eventArgs.RoutingKey,
                            body: retryBody);
                    }
                    else
                    {
                        var dlqJson = JsonSerializer.Serialize(envelope);
                        var dlqBody = Encoding.UTF8.GetBytes(dlqJson);

                        await channel.BasicPublishAsync(
                            exchange: "",
                            routingKey: QueueNames.Dlq,
                            body: dlqBody);
                    }
                }

                await channel.BasicAckAsync(eventArgs.DeliveryTag, false);
            }
        };

        await channel.BasicConsumeAsync(QueueNames.Email, false, consumer);
        await channel.BasicConsumeAsync(QueueNames.Sms, false, consumer);
        await channel.BasicConsumeAsync(QueueNames.Otp, false, consumer);

        logger.LogInformation("Worker started...");

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}