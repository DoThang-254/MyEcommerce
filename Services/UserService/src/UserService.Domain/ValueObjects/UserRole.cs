namespace UserService.Domain.ValueObjects;

public class UserRole
{
    public Guid Id { get; private set; }
    public string RoleName { get; private set; }

    public UserRole(string roleName)
    {
        Id = Guid.NewGuid();
        RoleName = roleName;
    }

    private UserRole() { }

    public override bool Equals(object? obj)
    {
        if (obj is UserRole other)
            return RoleName == other.RoleName;
        return false;
    }

    public override int GetHashCode() => RoleName.GetHashCode();
}