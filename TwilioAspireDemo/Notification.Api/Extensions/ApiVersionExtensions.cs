using Asp.Versioning;
using Asp.Versioning.Builder;

namespace Notification.Api.Extensions;

public static class ApiVersionExtensions
{
    public static IServiceCollection AddVersion(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        })
          .AddApiExplorer(options =>
          {
              options.GroupNameFormat = "'v'V";
              options.SubstituteApiVersionInUrl = true;
          });

        return services;
    }

    public static RouteGroupBuilder MapVersionedGroup(this IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
          .HasApiVersion(new ApiVersion(1))
          .ReportApiVersions()
          .Build();


        var rout = app.MapGroup("/api/v{version:version}")
            .WithTags("v1")
            .WithApiVersionSet(apiVersionSet);

        return rout;
    }
}
