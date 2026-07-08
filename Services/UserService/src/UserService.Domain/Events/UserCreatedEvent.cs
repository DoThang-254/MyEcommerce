using Shared.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserService.Domain.Events
{
    public record UserCreatedEvent : IDomainEvent
    {
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    }
}
