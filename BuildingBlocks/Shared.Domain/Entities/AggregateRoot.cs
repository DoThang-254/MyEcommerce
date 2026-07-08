using Shared.Domain.Events;

namespace Shared.Domain.Entities
{
    public abstract class AggregateRoot<TId> : BaseEntity<TId>, IAggregateRoot
    {
        public TId Id { get; protected set; }

        // Danh sách nội bộ lưu trữ các sự kiện xảy ra trong nghiệp vụ
        private readonly List<IDomainEvent> _domainEvents = new();

        // Read-only collection để lớp bên ngoài (như DbContext) có thể đọc
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
