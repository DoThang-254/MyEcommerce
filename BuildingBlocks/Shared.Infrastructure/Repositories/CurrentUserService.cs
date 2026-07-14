using Microsoft.AspNetCore.Http;
using Shared.Application.Common.Interfaces;
using System.Security.Claims;

namespace Shared.Infrastructure.Repositories
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;
                if (context == null) return Guid.Empty;

                // 1. Ưu tiên đọc từ Claims của Token (ASP.NET Core mặc định sẽ nhét vào đây)
                var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                               ?? context.User.FindFirst("sub")?.Value
                               ?? context.User.FindFirst("id")?.Value;

                if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out Guid claimId))
                {
                    return claimId;
                }

                // 2. Nếu Claims không có, thử tìm trong Header (dành cho trường hợp chạy qua API Gateway)
                var headerIdString = context.Request.Headers["X-User-Id"].FirstOrDefault();
                if (Guid.TryParse(headerIdString, out Guid headerId))
                {
                    return headerId;
                }

                // 3. Nếu cả 2 đều không có, chấp nhận trả về Guid.Empty
                return Guid.Empty;
            }
        }

        public string? CorrelationId => _httpContextAccessor.HttpContext?.Request.Headers["X-Correlation-Id"].FirstOrDefault()
            ?? _httpContextAccessor.HttpContext?.TraceIdentifier;

        public string? IpAddress => _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
    }
}
