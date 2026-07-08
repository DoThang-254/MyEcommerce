namespace UserService.Application.Features.Users.Commands.Login;

public record UserInfoResponse(
    Guid Id = default,
    string Username = "",
    string Email = "",  
    string FullName = "",
    string Role = ""
);

public record LoginResponse(
    string Token,
    DateTime Expiry,
    UserInfoResponse User
);
