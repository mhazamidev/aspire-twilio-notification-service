using FluentValidation;

namespace Notification.Application.Features.Whatsapp;

public class SendWhatsappCommandValidator : AbstractValidator<SendWhatsappCommand>
{
    public SendWhatsappCommandValidator()
    {
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+\d{1,3}\d{7,}$").WithMessage("Phone number must be in international format (e.g., +1234567890).");
        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required.")
            .MaximumLength(4096).WithMessage("Message cannot exceed 4096 characters.");
    }
}
