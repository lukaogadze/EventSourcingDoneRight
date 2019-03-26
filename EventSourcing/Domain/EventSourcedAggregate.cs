using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rafaela.DDD;

namespace EventSourcing.Domain
{
    public abstract class EventSourcedAggregate : Entity<Guid>
    {
        protected abstract void Apply(DomainEvent @event);
        public abstract ReadOnlyCollection<DomainEvent> Changes { get;}
        public int DomainEventVersion { get; protected set; }
        public int StoredEventVersion { get; protected set; }
        
        protected EventSourcedAggregate(Guid id) : base(id)
        {            
        }
    }
}