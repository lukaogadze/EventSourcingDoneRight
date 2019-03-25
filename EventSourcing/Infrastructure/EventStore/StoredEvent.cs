using System;
using EventSourcing.Domain;

namespace EventSourcing.Infrastructure.EventStore
{
    public class StoredEvent
    {
        public StoredEvent()
        {
            
        }
        public string Id { get; private set; }
        public DomainEvent Event { get; private set; }
        public int Version { get; private set; }
        public string EventStreamId { get; private set; }

        public StoredEvent(string eventStreamId, DomainEvent @event, int lastStoredEventVersion)
        {
            EventStreamId = eventStreamId ?? throw new ArgumentNullException(nameof(eventStreamId));
            if (lastStoredEventVersion <= 0)
            {
                throw new InvalidOperationException($"{nameof(lastStoredEventVersion)} should not be negative or zero");
            }

            Id = Guid.NewGuid().ToString();
            Event = @event ?? throw new ArgumentNullException(nameof(@event));
            Version = lastStoredEventVersion;
            
        }
    }
}