using MediatR;
using Notification.Domain.MessageLogs.Enums;

namespace Notification.Application.Features.SentOtp;

public record SendOtpCommand(string Recipient) : IRequest;

