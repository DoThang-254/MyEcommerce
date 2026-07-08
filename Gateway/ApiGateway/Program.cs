using Shared.API;
using System.Security.Claims;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

// 1. Gọi Shared API để đăng ký các dịch vụ cơ bản (JWT, Swagger, Exception Handling...)
// Truyền Assembly hiện tại vào trong trường hợp Shared API cần dùng tới
builder.AddSharedDefaults(typeof(Program).Assembly, enableControllers: false);
// 2. Đăng ký các dịch vụ RIÊNG của API Gateway
// Lưu ý: Mặc dù Shared API đã gọi AddAuthorization(), bạn vẫn có thể gọi lại 
// để thêm các Policy đặc thù của Gateway. ASP.NET Core sẽ tự động gộp (merge) chúng lại.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AuthenticatedPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});

builder.Services.AddHttpClient();

// Cấu hình YARP Reverse Proxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms(builderContext =>
    {
        // Thêm custom transform cho mọi request
        builderContext.AddRequestTransform(transformContext =>
        {
            // Lấy userId từ Claims của user đã được Gateway xác thực
            var userId = transformContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? transformContext.HttpContext.User.FindFirst("sub")?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                // Nhét vào header truyền xuống OrderService
                transformContext.ProxyRequest.Headers.Add("X-User-Id", userId);
            }

            return ValueTask.CompletedTask;
        });
    });

var app = builder.Build();

// 3. Gọi Pipeline dùng chung (sẽ tự động chạy UseAuthentication, UseAuthorization, Exception Middleware...)
app.UseSharedDefaults(enableControllers: false);
// 4. Map luồng riêng của Gateway (YARP)
app.MapReverseProxy();

// Thêm endpoint để test xem Gateway có đang chạy không
app.MapGet("/", () => "API Gateway is running on Azure Container Instances!");

app.Run();