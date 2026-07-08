using UserService.Domain.Interfaces;
using BCrypt.Net;

namespace UserService.Infrastructure.Repositories
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            // Tự động sinh Salt và mã hóa mật khẩu.
            // Mặc định work factor là 11, đủ an toàn và cân bằng về hiệu năng.
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Verify(string passwordHash, string inputPassword)
        {
            try
            {
                // Hàm Verify của BCrypt sẽ tự bóc tách Salt từ passwordHash 
                // và tiến hành so sánh với inputPassword
                return BCrypt.Net.BCrypt.Verify(inputPassword, passwordHash);
            }
            catch (BcryptAuthenticationException)
            {
                return false;
            }
        }
    }
}
