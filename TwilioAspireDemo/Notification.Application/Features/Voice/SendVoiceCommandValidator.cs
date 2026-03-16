using FluentValidation;

namespace Notification.Application.Features.Voice;

public class SendVoiceCommandValidator : AbstractValidator<SendVoiceCommand>
{
    public SendVoiceCommandValidator()
    {
        RuleFor(x => x.phone)
            .NotEmpty()
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Invalid phone number format.");
    }
}
