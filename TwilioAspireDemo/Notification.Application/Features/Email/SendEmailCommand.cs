using MediatR;

namespace Notification.Application.Features.Email;

public record SendEmailCommand(string Email, string Message) : IRequest<bool>;
