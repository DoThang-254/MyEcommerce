using Microsoft.Extensions.DependencyInjection;
using Shared.Application;
using System.Reflection;

namespace UserService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddSharedApplication(assembly);

        return services;
    }
}

