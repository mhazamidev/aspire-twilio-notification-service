using Carter;
using MediatR;
using Notification.Api.Base;
using Notification.Application.Features.Whatsapp;

namespace Notification.Api.Endpoints;

public class WhatsappModule(ISender sender) : BaseModule(sender), ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var mapGroup = app.MapGroup("/whatsapp").WithTags("Whatsapp");

        mapGroup.MapPost("/send", async (SendWhatsappCommand request) =>
        {
            return await Response(request);
        })
         .WithName("Send")
         .Produces(StatusCodes.Status200OK)
         .Produces(StatusCodes.Status400BadRequest);
    }
}
