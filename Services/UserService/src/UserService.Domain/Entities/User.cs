using Shared.Domain.Entities;
using Shared.Domain.ValueObjects;

namespace UserService.Domain.Entities
{
    public class User : AggregateRoot<Guid>
    {
        public string Username { get; private set; } = string.Empty;
        public EmailAddress Email { get; private set; }
        public string PasswordHash { get; private set; } = string.Empty;
        public string FullName { get; private set; } = string.Empty;
        public PhoneNumber PhoneNumber { get; private set; }

        // Thay đổi ở đây: Chỉ còn 1 Role
        public string Role { get; private set; } = string.Empty;

        public bool IsActive { get; private set; } = true;
        public DateTime? LastLoginAt { get; private set; }

        private User() { }

        public User(Guid id, string username, string email, string passwordHash, string fullName, string role, string phoneNumber)
        {
            Id = id;
            Username = username;
            Email = EmailAddress.Create(email);
            PasswordHash = passwordHash;
            FullName = fullName;
            PhoneNumber = PhoneNumber.Create(phoneNumber);
            IsActive = true;

            // Gán role trực tiếp, kiểm tra nếu không hợp lệ thì để mặc định
            Role = Constants.Roles.AllRoles.Contains(role) ? role : Constants.Roles.User;
        }

        public void UpdateRole(string newRole)
        {
            if (Constants.Roles.AllRoles.Contains(newRole))
            {
                Role = newRole;
            }
        }

        public void UpdateLastLogin() => LastLoginAt = DateTime.UtcNow;
        public void Deactivate() => IsActive = false;
    }
}