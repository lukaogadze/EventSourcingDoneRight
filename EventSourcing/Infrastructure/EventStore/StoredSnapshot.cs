using System;

namespace EventSourcing.Infrastructure.EventStore
{
    public class StoredSnapshot
    {
        public Guid Id { get; }
        public Guid EventStreamId { get; }
        public object Snapshot { get; }
        public DateTimeOffset Created { get; }

        public StoredSnapshot(Guid eventStreamId, object snapshot)
        {
            Id = Guid.NewGuid();

            if (eventStreamId == default(Guid))
            {
                throw new ArgumentException("eventStreamId should be initialized", nameof(eventStreamId));
            }

            Snapshot = snapshot ?? throw new ArgumentNullException(nameof(snapshot));
            EventStreamId = eventStreamId;
            
            Created = DateTimeOffset.UtcNow;
        }
    }
}