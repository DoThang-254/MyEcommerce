using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces
{
    public interface IJwtRepository
    {
        string GenerateToken(User user);
    }
}
