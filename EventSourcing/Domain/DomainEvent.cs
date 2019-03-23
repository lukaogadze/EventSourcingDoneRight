using System;

namespace EventSourcing.Domain
{
    public abstract class DomainEvent
    {
        public DateTimeOffset OccurredOn { get; }

        public DomainEvent()
        {
            OccurredOn = DateTimeOffset.UtcNow;
        } 
    }
}