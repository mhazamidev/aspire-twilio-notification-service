using FluentValidation;

namespace Notification.Application.Features.SentOtp;

public class SendOtpCommandValidator : AbstractValidator<SendOtpCommand>
{
    public SendOtpCommandValidator()
    {
        RuleFor(x => x.Recipient)
             .NotEmpty().WithMessage("Recipient is required.");     
    }
}
