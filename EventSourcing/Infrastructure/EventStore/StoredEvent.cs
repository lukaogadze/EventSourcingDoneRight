using System;
using EventSourcing.Domain;

namespace EventSourcing.Infrastructure.EventStore
{
    public class StoredEvent
    {
        public Guid Id { get; }
        public DomainEvent Event { get; }
        public long Version { get; }
        public Guid EventStreamId { get;  }

        public StoredEvent(Guid eventStreamId, DomainEvent @event, long lastStoredEventVersion)
        {
            if (default(Guid) == eventStreamId)
            {
                throw new InvalidOperationException($"{nameof(eventStreamId)} should be initialized.");
            }
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }
            if (lastStoredEventVersion <= 0)
            {
                throw new InvalidOperationException($"{nameof(lastStoredEventVersion)} should not be negative or zero");
            }

            Id = Guid.NewGuid();
            Event = @event;
            Version = lastStoredEventVersion;
            EventStreamId = eventStreamId;
        }
    }
}