using MediatR;
using Microsoft.Extensions.Options;
using Notification.Application.Channels;
using Notification.Domain.MessageLogs.Enums;
using Notification.Infrastructure.Configurations;
using Notification.Infrastructure.Persistence.Interfaces;

namespace Notification.Application.Features.VerifyOtp;

public class VerifyOtpCommandHandler(
    IOtpChannel _provider,
    IOptions<TwilioConfigs> _options) : IRequestHandler<VerifyOtpCommand, bool>
{
    public async Task<bool> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        return await _provider.VerifyAsync(request.PhoneNumber, request.Code);
    }
}
