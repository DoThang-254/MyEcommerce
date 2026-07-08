using Shared.Application.Features.Messaging;

namespace UserService.Application.Features.Users.Commands.Login;

public class LoginCommand : ICommand<LoginResponse>
{
    public string Email { get; init; }
    public string Password { get; init; }
}

