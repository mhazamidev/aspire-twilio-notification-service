using MediatR;

namespace Notification.Application.Features.Whatsapp;

public record SendWhatsappCommand(string Phone, string Message) : IRequest<bool>;
