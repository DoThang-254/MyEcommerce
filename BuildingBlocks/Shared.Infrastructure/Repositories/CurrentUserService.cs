using Shared.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Shared.Infrastructure.Repositories
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId => Guid.TryParse(_httpContextAccessor.HttpContext?.Request.Headers["X-User-Id"].FirstOrDefault(), out Guid userId) ? userId : Guid.Empty;
    }
}
