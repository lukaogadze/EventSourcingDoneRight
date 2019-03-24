using System;
using EventSourcing.Domain;

namespace EventSourcing.Infrastructure.EventStore
{
    public class EventStream
    {
        public Guid Id { get; }
        public long LastStoredEventVersion { get; private set; }
        public  Guid AggregateId { get; }        
        public EventStream(Guid aggregateId)
        {
            Id = Id;
            AggregateId = aggregateId;            
        }

        public StoredEvent RegisterStoredEvent(Guid eventStreamId, DomainEvent domainEvent)
        {
            LastStoredEventVersion++;
            return new StoredEvent(eventStreamId, domainEvent, LastStoredEventVersion);
        }
    }
}