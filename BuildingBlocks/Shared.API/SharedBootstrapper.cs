using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Shared.API.Middlewares;
using System.Reflection;
using System.Text;

namespace Shared.API;

public static class SharedBootstrapper
{
    // Thêm tham số enableControllers
    public static WebApplicationBuilder AddSharedDefaults(this WebApplicationBuilder builder, Assembly assembly, bool enableControllers = true)
    {
        if (enableControllers)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //builder.Services.AddOpenApi();
        }

        builder.Services.AddHttpContextAccessor();
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"])),
                    ValidateLifetime = true
                };
            });
        builder.Services.AddAuthorization();

        return builder;
    }

    // Thêm tham số enableControllers
    public static WebApplication UseSharedDefaults(this WebApplication app, bool enableControllers = true)
    {
        if (app.Environment.IsDevelopment() && enableControllers)
        {
            //app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        // app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        // Chỉ map controller nếu được cho phép
        if (enableControllers)
        {
            app.MapControllers();
        }

        return app;
    }
}