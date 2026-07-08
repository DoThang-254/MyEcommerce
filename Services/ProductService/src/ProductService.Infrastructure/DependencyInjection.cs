using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Domain.Interfaces;
using ProductService.Infrastructure.Data;
using ProductService.Infrastructure.Repositories;
using Shared.Infrastructure;

namespace ProductService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Cấu hình DbContext cho Product Service
        services.AddDbContext<ProductDbContext>((sp, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        // 2. DÙNG LẠI DI TỪ SHARED: Đăng ký UnitOfWork tự động cho ProductDbContext
        services.AddSharedInfrastructure<ProductDbContext>();

        // 3. Đăng ký các Repository riêng của Product Service
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}

