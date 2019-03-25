using System;
using EventSourcing.Domain;

namespace EventSourcing.Infrastructure.EventStore
{
    public class EventStream
    {
        public EventStream()
        {
            
        }
        public string Id { get; private set; }
        public Guid AggregateId { get; private set;}
        public int LastStoredEventVersion { get; private set; }    
        public EventStream(string eventStreamId, Guid aggregateId)
        {
            Id = eventStreamId ?? throw new ArgumentNullException(nameof(eventStreamId));            
            if (default(Guid) == aggregateId)
            {
                throw new InvalidOperationException($"{nameof(aggregateId)} must be initialized");
            }            
            AggregateId = aggregateId;            
        }

        public StoredEvent RegisterStoredEvent(DomainEvent domainEvent)
        {
            if (domainEvent == null)
            {
                throw new ArgumentNullException(nameof(domainEvent));
            }

            LastStoredEventVersion++;
            return new StoredEvent(Id, domainEvent, LastStoredEventVersion);
        }
    }
}