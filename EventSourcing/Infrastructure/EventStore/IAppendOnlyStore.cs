using System;
using System.Collections.Generic;
using EventSourcing.Domain;
using Rafaela.Functional;

namespace EventSourcing.Infrastructure.EventStore
{
    public interface IAppendOnlyStore
    {
        void Append(Guid eventStreamId, StoredEvent storedEvent, Option<long> expectedVersion);
        void Create(Guid eventStreamId, IEnumerable<DomainEvent> domainEvents);        
        Option<IList<StoredEvent>> GetStoredEvents(Guid eventStreamId, long afterVersion, long maxCount);
        void AddSnapshot<T>(Guid eventStreamId, T snapshot);
        Option<T> GetLatestSnapshot<T>(Guid eventStreamId) where T : class;        
    }
}