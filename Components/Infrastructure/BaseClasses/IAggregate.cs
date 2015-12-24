using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Satrabel.OpenContent.Components.Infrastructure
{
    public interface IAggregate
    {
        Guid Id { get; }

        bool HasPendingChanges { get; }

        IEnumerable<DomainEvent> GetUncommittedEvents();

        void ClearUncommittedEvents();
    }
}
