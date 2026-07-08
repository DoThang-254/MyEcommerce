using Shared.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces
{
    public interface IUserRepository : IBaseRepository<User , Guid>
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}
