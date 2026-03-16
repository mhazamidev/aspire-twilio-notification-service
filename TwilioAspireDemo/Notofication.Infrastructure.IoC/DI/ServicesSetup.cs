using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Services;

namespace Notofication.Infrastructure.IoC.DI;

public static class ServicesSetup
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<OtpGenerator>();

        return services;
    }
}
