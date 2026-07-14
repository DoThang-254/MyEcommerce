using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Moq;
using Shared.Application.Common.Interfaces;

namespace UserService.Infrastructure.Data;

public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
{
    public UserDbContext CreateDbContext(string[] args)
    {
        // 1. Lấy đường dẫn tuyệt đối đến thư mục chứa solution/project
        // Cách này sẽ tìm ngược lên từ thư mục bin cho đến khi thấy thư mục API
        var currentDirectory = Directory.GetCurrentDirectory();

        // Thử tìm file appsettings.json trong thư mục API
        // Điều chỉnh đường dẫn này cho khớp với cấu trúc thư mục thật của bạn
        var apiPath = Path.GetFullPath(Path.Combine(currentDirectory, "src", "UserService.API"));

        // Kiểm tra nếu không thấy thư mục API (trường hợp bạn đang đứng ở bên trong src)
        if (!Directory.Exists(apiPath))
        {
            apiPath = Path.GetFullPath(Path.Combine(currentDirectory, "..", "UserService.API"));
        }

        Console.WriteLine($"EF Design-time logic is looking for settings in: {apiPath}");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiPath) // Trỏ thẳng vào thư mục chứa appsettings.json
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Could not find 'DefaultConnection' in appsettings.json. Path: " + apiPath);
        }

        var builder = new DbContextOptionsBuilder<UserDbContext>();
        builder.UseSqlServer(connectionString);

        // Mock Mediator vì Design-time không cần bắn event
        var mockMediator = new Mock<IMediator>();

        var mockCurrentUserService = new Mock<ICurrentUserService>();
        // Set cứng UserId là "System-Migration" để nếu có lưu seed data thì biết là do chạy lệnh EF
        mockCurrentUserService.Setup(m => m.UserId).Returns(Guid.NewGuid());

        return new UserDbContext(builder.Options, mockMediator.Object, mockCurrentUserService.Object);
    }
}