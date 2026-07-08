using FluentValidation;

namespace UserService.Application.Features.Users.Commands.Login
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không được để trống.")
                .EmailAddress().WithMessage("Email không hợp lệ.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu không được để trống.")
                // Không nên giới hạn MaxLength quá thấp cho mật khẩu vì lý do bảo mật
                .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự.");
        }
    }
}
                
