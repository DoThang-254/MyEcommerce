using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserService.Domain.Exceptions
{
    public class AuthenticationException : DomainException
    {
        // Sử dụng một thông báo chung để bảo mật
        public AuthenticationException()
            : base("Invalid username or password.", "Unauthorized")
        {
        }

        public AuthenticationException(string message)
            : base(message, "Unauthorized")
        {
        }
    }
}
