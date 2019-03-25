using System;

namespace EventSourcing.Infrastructure.EventStore
{
    public class StoredSnapshot
    {
        public string Id { get; }
        public string EventStreamId { get; }
        public object Snapshot { get; }
        public DateTimeOffset Created { get; }

        public StoredSnapshot(string eventStreamId, object snapshot)
        {
            Id = Guid.NewGuid().ToString();

            Snapshot = snapshot ?? throw new ArgumentNullException(nameof(snapshot));
            EventStreamId = eventStreamId ?? throw new ArgumentNullException(nameof(eventStreamId));
            
            Created = DateTimeOffset.UtcNow;
        }
    }
}