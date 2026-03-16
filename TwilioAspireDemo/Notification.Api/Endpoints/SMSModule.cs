using Carter;
using MediatR;
using Notification.Api.Base;
using Notification.Application.Features.Sms;

namespace Notification.Api.Endpoints;

public class SMSModule(ISender sender) : BaseModule(sender), ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var mapGroup = app.MapGroup("/sms").WithTags("SMS");

        mapGroup.MapPost("/send", async (SendSMSCommand request) =>
        {
            return await Response(request);
        })
          .WithName("Send")
          .Produces(StatusCodes.Status200OK)
          .Produces(StatusCodes.Status400BadRequest);

    }
}
