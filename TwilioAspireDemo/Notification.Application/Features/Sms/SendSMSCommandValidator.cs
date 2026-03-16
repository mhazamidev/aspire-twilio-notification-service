using FluentValidation;

namespace Notification.Application.Features.Sms;

public class SendSMSCommandValidator : AbstractValidator<SendSMSCommand>
{
    public SendSMSCommandValidator()
    {
        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Invalid phone number format.");

        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("Message content is required.")
            .MaximumLength(160)
            .WithMessage("Message content cannot exceed 160 characters.");
    }
}
