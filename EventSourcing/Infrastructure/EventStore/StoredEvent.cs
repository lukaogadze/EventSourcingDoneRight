using System;
using EventSourcing.Domain;

namespace EventSourcing.Infrastructure.EventStore
{
    public class StoredEvent
    {
        public StoredEvent()
        {
            
        }
        public string Id { get; protected set; }
        public DomainEvent Event { get; protected set; }
        public int Version { get; protected set; }
        public string EventStreamId { get; protected set; }

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