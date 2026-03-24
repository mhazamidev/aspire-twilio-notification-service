using BuildingBlocks.Contracts.Contracts.Notification;
using BuildingBlocks.Messaging;
using Notification.Infrastructure.Persistence.Interfaces;
using Notification.Worker.Processors;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Notification.Worker.Workers;



public class NotificationWorker(
    ILogger<NotificationWorker> logger,
    IConnection connection,
    IServiceScopeFactory scopeFactory,
    IRetryHandler retryHandler) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = await connection.CreateChannelAsync();

       
        await channel.ExchangeDeclareAsync(
            exchange: Exchanges.Notification,
            type: ExchangeType.Topic,
            durable: true);

        await channel.ExchangeDeclareAsync(
            exchange: Exchanges.DeadLetter,
            type: ExchangeType.Direct,
            durable: true);

      
        await channel.QueueDeclareAsync(QueueNames.Email, true, false, false);
        await channel.QueueDeclareAsync(QueueNames.Sms, true, false, false);
        await channel.QueueDeclareAsync(QueueNames.Otp, true, false, false);

        await channel.QueueBindAsync(QueueNames.Email, Exchanges.Notification, RoutingKeys.Email);
        await channel.QueueBindAsync(QueueNames.Sms, Exchanges.Notification, RoutingKeys.Sms);
        await channel.QueueBindAsync(QueueNames.Otp, Exchanges.Notification, RoutingKeys.Otp);

    
        var retryArgs = new Dictionary<string, object>
        {
            { "x-message-ttl", 10000 }, // 10s delay
            { "x-dead-letter-exchange", Exchanges.Notification }
        };

        await channel.QueueDeclareAsync(
            QueueNames.Retry,
            true,
            false,
            false,
            retryArgs);

      
        await channel.QueueDeclareAsync(QueueNames.Dlq, true, false, false);

        await channel.QueueBindAsync(
            QueueNames.Dlq,
            Exchanges.DeadLetter,
            RoutingKeys.Dlq);

    
        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (sender, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);

            NotificationEnvelope? envelope = null;

            try
            {
                envelope = JsonSerializer.Deserialize<NotificationEnvelope>(json);

                if (envelope == null)
                    throw new Exception("Invalid message");

                envelope.RoutingKey ??= eventArgs.RoutingKey;

                using var scope = scopeFactory.CreateScope();
                var processor = scope.ServiceProvider.GetRequiredService<NotificationProcessor>();

                await processor.ProcessAsync(envelope);

                await channel.BasicAckAsync(eventArgs.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Error processing message. RetryCount: {RetryCount}",
                    envelope?.RetryCount);

                if (envelope != null)
                {
                    using var scope = scopeFactory.CreateScope();

                    var retryHandler = scope.ServiceProvider.GetRequiredService<IRetryHandler>();

                    await retryHandler.HandleAsync(envelope);
                }
                else
                {
                    await channel.BasicPublishAsync(
                        exchange: Exchanges.DeadLetter,
                        routingKey: RoutingKeys.Dlq,
                        body: body);

                    await channel.BasicAckAsync(eventArgs.DeliveryTag, false);
                }
            }
        };

        await channel.BasicConsumeAsync(QueueNames.Email, false, consumer);
        await channel.BasicConsumeAsync(QueueNames.Sms, false, consumer);
        await channel.BasicConsumeAsync(QueueNames.Otp, false, consumer);

        logger.LogInformation("Notification Worker started...");

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
