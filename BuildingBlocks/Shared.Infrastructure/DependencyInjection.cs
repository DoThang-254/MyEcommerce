using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Interfaces;
using Shared.Infrastructure.Repositories;
using Shared.Infrastructure.Storage;

namespace Shared.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedInfrastructure<TContext>(this IServiceCollection services, IConfiguration configuration)
        where TContext : DbContext
    {
        services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        var account = new Account(
                configuration["Cloudinary:CloudName"],
                configuration["Cloudinary:ApiKey"],
                configuration["Cloudinary:ApiSecret"]
            );

        var cloudinary = new Cloudinary(account);

        // 2. Đăng ký dịch vụ vào DI Container
        services.AddSingleton(cloudinary);
        services.AddScoped<IFileStorageService, CloudinaryStorageService>();
        return services;
    }
}

