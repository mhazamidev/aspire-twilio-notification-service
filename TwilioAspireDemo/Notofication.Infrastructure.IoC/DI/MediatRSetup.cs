using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Pipelines;
using FluentValidation;

namespace Notofication.Infrastructure.IoC.DI;

public static class MediatRSetup
{
    public static IServiceCollection AddMediatRSetup(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ValidationBehaviour<,>).Assembly);
        })
          .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>))
          .AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));

        services.AddValidatorsFromAssembly(typeof(ValidationBehaviour<,>).Assembly);
        return services;
    }
}
