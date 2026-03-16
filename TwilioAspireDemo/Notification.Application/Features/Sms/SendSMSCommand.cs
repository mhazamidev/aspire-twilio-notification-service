using MediatR;

namespace Notification.Application.Features.Sms;

public record SendSMSCommand(string Phone, string Message) : IRequest<bool>;
