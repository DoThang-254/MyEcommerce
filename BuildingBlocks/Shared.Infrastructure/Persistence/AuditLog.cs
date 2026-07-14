using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Infrastructure.Persistence
{
    public class AuditLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime OccurredAtUtc { get; set; }

        public string ServiceName { get; set; } = default!;
        public Guid? UserId { get; set; }
        public string? CorrelationId { get; set; }
        public string? IpAddress { get; set; }

        public string EntityName { get; set; } = default!;
        public string EntityId { get; set; } = default!;
        public string Action { get; set; } = default!; // Create / Update / Delete

        public string? OldValues { get; set; } // JSON
        public string? NewValues { get; set; } // JSON
    }
}
