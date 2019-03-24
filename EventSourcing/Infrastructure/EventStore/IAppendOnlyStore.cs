using System;
using System.Collections.Generic;
using EventSourcing.Domain;
using Rafaela.Functional;

namespace EventSourcing.Infrastructure.EventStore
{
    public interface IAppendOnlyStore
    {
        void Append(Guid eventStreamId, IEnumerable<DomainEvent> domainEvents, Option<int> expectedVersion);
        void Create(Guid aggregateId, IEnumerable<DomainEvent> domainEvents);        
        Option<IEnumerable<StoredEvent>> GetStoredEvents(Guid eventStreamId, int afterVersion, int maxCount);
        void AddSnapshot<T>(Guid eventStreamId, T snapshot);
        Option<T> GetLatestSnapshot<T>(Guid eventStreamId) where T : class;        
    }
}