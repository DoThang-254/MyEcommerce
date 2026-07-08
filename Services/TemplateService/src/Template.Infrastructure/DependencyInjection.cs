using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure;

namespace Template.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Cấu hình DbContext cho User Service
        //services.AddDbContext<UserDbContext>((sp, options) =>
        //{
        //    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        //});

        //// 2. DÙNG LẠI DI TỪ SHARED: Đăng ký UnitOfWork tự động cho UserDbContext
        //services.AddSharedInfrastructure<UserDbContext>();

        //// 3. Đăng ký các Repository riêng của User Service
        //services.AddScoped<IUserRepository , UserRepository>();
        //services.AddScoped<IJwtRepository, JwtRepository>();
        //services.AddSingleton<IPasswordHasher, PasswordHasher>();

        return services;
    }
}

