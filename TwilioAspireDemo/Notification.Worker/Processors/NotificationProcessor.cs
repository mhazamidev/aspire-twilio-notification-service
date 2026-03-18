using BuildingBlocks.Contracts.Notification.Contracts;
using BuildingBlocks.Utility;
using MediatR;
using Notification.Application.Features.Email;
using Notification.Application.Features.SentOtp;
using Notification.Application.Features.Sms;
using Notification.Domain.MessageLogs.Enums;

namespace Notification.Worker.Processors;

public class NotificationProcessor
{
    private readonly ISender _sender;
    private readonly ILogger<NotificationProcessor> _logger;

    public NotificationProcessor(ISender sender, ILogger<NotificationProcessor> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    public async Task ProcessAsync(NotificationEnvelope envelope)
    {
        MessageChannel messageChannel;

        try
        {
            messageChannel = envelope.Payload.Channel.GetEnumValue<MessageChannel>();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "MessageChannel Converting Error");
            throw;
        }

        switch (messageChannel)
        {
            case MessageChannel.Email:
                await _sender.Send(new SendEmailCommand(envelope.Payload.Recipient, envelope.Payload.Content));
                break;
            case MessageChannel.Sms:
                await _sender.Send(new SendSMSCommand(envelope.Payload.Recipient, envelope.Payload.Content));
                break;
            case MessageChannel.Otp:
                await _sender.Send(new SendOtpCommand(envelope.Payload.Recipient));
                break;
        }

    }
}
