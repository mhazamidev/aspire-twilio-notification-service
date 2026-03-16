using Carter;
using MediatR;
using Notification.Api.Base;
using Notification.Application.Features.Voice;

namespace Notification.Api.Endpoints;

public class VoiceModule(ISender sender) : BaseModule(sender), ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var mapGroup = app.MapGroup("/voice").WithTags("Voice");

        mapGroup.MapPost("/send", async (SendVoiceCommand request) =>
        {
            return await Response(request);
        })
          .WithName("Send")
          .Produces(StatusCodes.Status200OK)
          .Produces(StatusCodes.Status400BadRequest);
    }
}
