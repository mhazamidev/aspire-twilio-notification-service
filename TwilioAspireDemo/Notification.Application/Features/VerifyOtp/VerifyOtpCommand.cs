using MediatR;

namespace Notification.Application.Features.VerifyOtp;

public record VerifyOtpCommand(string PhoneNumber, string Code) : IRequest<bool>;

