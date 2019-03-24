using System;
using EventSourcing.Domain;

namespace EventSourcing.Infrastructure.EventStore
{
    public class EventStream
    {
        public Guid Id { get; }
        public Guid AggregateId { get; }
        public long LastStoredEventVersion { get; private set; }    
        public EventStream(Guid aggregateId)
        {
            Id = Guid.NewGuid();
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