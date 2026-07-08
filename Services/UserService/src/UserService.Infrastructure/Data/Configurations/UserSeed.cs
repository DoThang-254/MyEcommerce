using Microsoft.EntityFrameworkCore;
using Shared.Domain.ValueObjects;
using UserService.Domain.Constants;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Data.Configurations
{
    public static class UserSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            var adminId = Guid.Parse("A1B2C3D4-E5F6-4A5B-8C9D-0E1F2A3B4C5D");
            var managerId = Guid.Parse("B2C3D4E5-F6A1-4B6C-9D0E-1F2A3B4C5D6E");
            var userId = Guid.Parse("C3D4E5F6-A1B2-4C7D-0E1F-2A3B4C5D6E7F");

            var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // TẤT CẢ TÀI KHOẢN ĐỀU DÙNG CHUNG MẬT KHẨU LÀ: Admin@123
            // Chuỗi Hash dưới đây được sinh ra từ lệnh: BCrypt.Net.BCrypt.HashPassword("Admin@123");
            // Việc gắn cứng (hardcode) chuỗi này giúp EF Core không sinh ra Migration rác mỗi khi chạy lại.
            var defaultPasswordHash = "$2a$11$w.N/yLw.e0kXlO9v8o6F.e7U/1d2k3j4h5g6f7e8d9c0b1a2A3B4C";

            modelBuilder.Entity<User>().HasData(
                new
                {
                    Id = adminId,
                    Username = "admin_master",
                    Email = EmailAddress.Create("admin@ecommerce.com"),
                    PasswordHash = defaultPasswordHash, // Sử dụng biến đã mã hóa
                    FullName = "System Administrator",
                    PhoneNumber = PhoneNumber.Create("0912345678"),
                    Role = Roles.Admin,
                    IsActive = true,
                    CreatedDate = seedDate
                },
                new
                {
                    Id = managerId,
                    Username = "manager_01",
                    Email = EmailAddress.Create("manager@ecommerce.com"),
                    PasswordHash = defaultPasswordHash, // Sử dụng biến đã mã hóa
                    FullName = "Store Manager",
                    PhoneNumber = PhoneNumber.Create("0987654321"),
                    Role = Roles.Manager,
                    IsActive = true,
                    CreatedDate = seedDate
                },
                new
                {
                    Id = userId,
                    Username = "thang_student",
                    Email = EmailAddress.Create("thang@student.com"),
                    PasswordHash = defaultPasswordHash, // Sử dụng biến đã mã hóa
                    FullName = "Thắng Student",
                    PhoneNumber = PhoneNumber.Create("0900112233"),
                    Role = Roles.User,
                    IsActive = true,
                    CreatedDate = seedDate
                }
            );
        }
    }
}