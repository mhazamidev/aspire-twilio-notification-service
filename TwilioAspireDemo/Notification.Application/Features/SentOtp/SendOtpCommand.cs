using MediatR;

namespace Notification.Application.Features.SentOtp;

public record SendOtpCommand(string Recipient) : IRequest;

