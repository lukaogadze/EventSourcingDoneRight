using System;
using System.Collections.Generic;
using EventSourcing.Domain;
using Rafaela.Functional;

namespace EventSourcing.Infrastructure.EventStore
{
    public interface IAppendOnlyStore
    {
        bool AppendToStream(string eventStreamId, IEnumerable<DomainEvent> domainEvents, Option<int> expectedVersion);
        void CreateStream(string eventStreamId, Guid aggregateId, IEnumerable<DomainEvent> domainEvents);        
        Option<IEnumerable<StoredEvent>> GetStoredEvents(string eventStreamId, int afterVersion, int maxCount);
        bool AddSnapshot<T>(string eventStreamId, T snapshot);
        Option<T> GetLatestSnapshot<T>(string eventStreamId) where T : class;        
    }
}