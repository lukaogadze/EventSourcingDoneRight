using System;
using System.Collections.Generic;
using Rafaela.DDD;

namespace EventSourcing.Domain
{
    public abstract class EventSourcedAggregate : Entity<Guid>
    {
        protected abstract void Apply(DomainEvent @event);
        public List<DomainEvent> Changes { get;}
        public long DomainEventVersion { get; protected set; }
        public long StoredEventVersion { get; set; }
        
        protected EventSourcedAggregate(Guid id) : base(id)
        {
            Changes = new List<DomainEvent>();
        }
    }
}