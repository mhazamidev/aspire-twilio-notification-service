using Carter;
using MediatR;
using Notification.Api.Base;
using Notification.Application.Features.Email;

namespace Notification.Api.Endpoints;

public class EmailModule(ISender sender) : BaseModule(sender), ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var mapGroup = app.MapGroup("/email").WithTags("Email");

        mapGroup.MapPost("/send", async (SendEmailCommand request) =>
        {
            return await Response(request);
        })
          .WithName("Send")
          .Produces(StatusCodes.Status200OK)
          .Produces(StatusCodes.Status400BadRequest);
    }
}
