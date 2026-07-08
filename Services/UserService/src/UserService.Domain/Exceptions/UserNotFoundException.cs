using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserService.Domain.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        // Khi tìm theo Id (Guid)
        public UserNotFoundException(Guid userId)
            : base("User", userId) // Sử dụng constructor (name, key) của base
        {
        }

        // Khi tìm theo Username
        public UserNotFoundException(string username)
            : base($"User with username \"{username}\" was not found.")
        {
        }
    }
}
