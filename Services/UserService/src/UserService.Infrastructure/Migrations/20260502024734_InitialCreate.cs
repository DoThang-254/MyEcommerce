using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Email", "FullName", "IsActive", "LastLoginAt", "LastModifiedBy", "LastModifiedDate", "PasswordHash", "PhoneNumber", "Role", "Username" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d"), null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@ecommerce.com", "System Administrator", true, null, null, null, "$2a$11$w.N/yLw.e0kXlO9v8o6F.e7U/1d2k3j4h5g6f7e8d9c0b1a2A3B4C", "0912345678", "Admin", "admin_master" },
                    { new Guid("b2c3d4e5-f6a1-4b6c-9d0e-1f2a3b4c5d6e"), null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "manager@ecommerce.com", "Store Manager", true, null, null, null, "$2a$11$w.N/yLw.e0kXlO9v8o6F.e7U/1d2k3j4h5g6f7e8d9c0b1a2A3B4C", "0987654321", "Manager", "manager_01" },
                    { new Guid("c3d4e5f6-a1b2-4c7d-0e1f-2a3b4c5d6e7f"), null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "thang@student.com", "Thắng Student", true, null, null, null, "$2a$11$w.N/yLw.e0kXlO9v8o6F.e7U/1d2k3j4h5g6f7e8d9c0b1a2A3B4C", "0900112233", "User", "thang_student" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
