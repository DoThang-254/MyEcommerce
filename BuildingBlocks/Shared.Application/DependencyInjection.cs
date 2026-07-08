using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Common.Behaviors;
using System.Reflection;

namespace Shared.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedApplication(this IServiceCollection services, Assembly assembly)
    {
        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        // AutoMapper
        services.AddAutoMapper(cfg => cfg.AddMaps(assembly));

        // FluentValidation
        services.AddValidatorsFromAssembly(assembly);

        // Pipeline Behaviors
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        return services;
    }
}
