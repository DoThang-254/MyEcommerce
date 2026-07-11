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

        public string? UserId => _httpContextAccessor.HttpContext?.Request.Headers["X-User-Id"].FirstOrDefault();

        public string? CorrelationId => _httpContextAccessor.HttpContext?.Request.Headers["X-Correlation-Id"].FirstOrDefault() 
            ?? _httpContextAccessor.HttpContext?.TraceIdentifier;

        public string? IpAddress => _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
    }
}
