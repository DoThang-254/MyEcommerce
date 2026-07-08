namespace UserService.Domain.Interfaces
{
    public interface IPasswordHasher
    {
        /// <summary>
        /// Mã hóa mật khẩu gốc thành chuỗi Hash (Dùng khi đăng ký user mới hoặc đổi mật khẩu)
        /// </summary>
        string Hash(string password);

        /// <summary>
        /// So sánh mật khẩu người dùng nhập vào với chuỗi Hash lưu trong Database (Dùng khi đăng nhập)
        /// </summary>
        bool Verify(string passwordHash, string inputPassword);
    }
}
