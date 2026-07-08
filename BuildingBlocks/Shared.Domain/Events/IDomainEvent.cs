using MediatR;

namespace Shared.Domain.Events
{
    public interface IDomainEvent : INotification
    {

        DateTime OccurredOn { get; }
    }
}
