using Shared.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Domain.Entities
{
    public interface IAggregateRoot
    {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
        void ClearDomainEvents();
    }
}
