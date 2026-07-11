using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Domain.Interfaces;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Repositories;
using Shared.Infrastructure;

namespace OrderService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Cấu hình DbContext cho OrderService Service
        services.AddDbContext<OrderDbContext>((sp, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        //// 2. DÙNG LẠI DI TỪ SHARED: Đăng ký UnitOfWork tự động cho OrderDbContext
        services.AddSharedInfrastructure<OrderDbContext>(configuration);

        //// 3. Đăng ký các Repository riêng của Order Service
        services.AddScoped<IOrderRepository, OrderRepository>();


        return services;
    }
}

