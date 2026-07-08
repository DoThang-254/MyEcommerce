using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Interfaces;
using Shared.Infrastructure.Repositories;

namespace Shared.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedInfrastructure<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}

