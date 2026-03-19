using Carter;
using MediatR;
using Notification.Api.Base;
using Notification.Application.Features.VerifyOtp;

namespace Notification.Api.Endpoints;

public class OtpModule(ISender sender) : BaseModule(sender), ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var mapGroup = app.MapGroup("/otp").WithTags("OTP");

        mapGroup.MapPost("/verify", async (VerifyOtpCommand request) =>
        {
            return await Response(request);
        })
            .WithName("Verify")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }
}
