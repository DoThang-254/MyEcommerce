using Shared.Domain.Events;

namespace ProductService.Domain.Events;

public record ProductCreatedEvent(Guid ProductId, string ProductName) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}

