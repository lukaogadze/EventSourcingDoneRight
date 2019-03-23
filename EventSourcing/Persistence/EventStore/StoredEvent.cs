using System;
using EventSourcing.Domain;

namespace EventSourcing.Persistence.EventStore
{
    public class StoredEvent
    {
        public Guid Id { get; }
        public DomainEvent Event { get; }
        public long Version { get; }
        public Guid EventStreamId { get;  }

        public StoredEvent(Guid eventStreamId, DomainEvent @event, long lastStoredEventVersion)
        {
            Id = Guid.NewGuid();
            Event = @event;
            Version = lastStoredEventVersion;
            EventStreamId = eventStreamId;
        }
    }
}