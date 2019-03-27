using System;

namespace EventSourcing.Infrastructure.EventStore
{
    public class StoredSnapshot
    {
        public string Id { get; protected set; }
        public string EventStreamId { get; protected set; }
        public object Snapshot { get; protected set; }
        public DateTimeOffset Created { get; protected set; }

        public StoredSnapshot(string eventStreamId, object snapshot)
        {
            Id = Guid.NewGuid().ToString();

            Snapshot = snapshot ?? throw new ArgumentNullException(nameof(snapshot));
            EventStreamId = eventStreamId ?? throw new ArgumentNullException(nameof(eventStreamId));
            
            Created = DateTimeOffset.UtcNow;
        }
    }
}