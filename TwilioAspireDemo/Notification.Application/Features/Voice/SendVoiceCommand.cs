using MediatR;

namespace Notification.Application.Features.Voice;

public record SendVoiceCommand(string phone) : IRequest<bool>;
