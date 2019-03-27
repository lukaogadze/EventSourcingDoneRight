using System;

namespace EventSourcing.Domain
{
    public abstract class DomainEvent
    {
        public DateTimeOffset OccurredOn { get; protected set; }

        public DomainEvent()
        {
            OccurredOn = DateTimeOffset.UtcNow;
        } 
    }
}