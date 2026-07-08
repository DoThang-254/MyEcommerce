namespace UserService.Domain.Constants
{
    public static class Roles
    {
        // Vai trò cao nhất, có quyền quản trị toàn bộ hệ thống
        public const string Admin = "Admin";

        // Quản trị viên cấp thấp hơn hoặc nhân viên quản lý nội dung/sản phẩm
        public const string Manager = "Manager";

        // Người dùng cuối (khách hàng)
        public const string User = "User";

        // Khách (chưa đăng ký hoặc chưa đăng nhập) - dùng cho các logic đặc biệt
        public const string Guest = "Guest";

        // Nhân viên hỗ trợ kỹ thuật hoặc chăm sóc khách hàng
        public const string Support = "Support";

        /// <summary>
        /// Danh sách tất cả các role để dùng cho việc seeding data hoặc validation
        /// </summary>
        public static readonly IReadOnlyList<string> AllRoles = new List<string>
        {
            Admin,
            Manager,
            User,
            Support,
            Guest
        };
    }
}