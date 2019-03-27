using System;
using System.Collections.Generic;
using EventSourcing.Domain;
using MongoDB.Driver;
using Rafaela.Functional;

namespace EventSourcing.Infrastructure.EventStore.Implementations
{
    public class MongoDbEventStore : IAppendOnlyStore
    {
        private readonly IMongoCollection<EventStream> _eventStreamCollection;
        private readonly IMongoCollection<StoredEvent> _storedEventCollection;
        private readonly IMongoCollection<StoredSnapshot> _storedSnapshotCollection;

        public MongoDbEventStore(IMongoCollection<EventStream> eventStreamCollection,
            IMongoCollection<StoredEvent> storedEventCollection,
            IMongoCollection<StoredSnapshot> storedSnapshotCollection)
        {
            _eventStreamCollection = eventStreamCollection;
            _storedEventCollection = storedEventCollection;
            _storedSnapshotCollection = storedSnapshotCollection;
        }

        public bool AppendToStream(string eventStreamId, IEnumerable<DomainEvent> domainEvents,
            Option<int> expectedVersion)
        {
            var eventStream = _eventStreamCollection.Find(x => x.Id == eventStreamId).FirstOrDefault();

            if (eventStream == null)
            {
                return false;
            }

            if (expectedVersion.IsSome)
            {
                CheckForConcurrency(expectedVersion.Value, eventStream.LastStoredEventVersion);
            }

            var storedEvents = new List<StoredEvent>();
            foreach (DomainEvent @event in domainEvents)
            {
                var storedEvent = eventStream.RegisterStoredEvent(@event);
                storedEvents.Add(storedEvent);
            }

            _storedEventCollection.InsertMany(storedEvents);

            _eventStreamCollection.FindOneAndReplace(e => e.Id == eventStream.Id, eventStream);

            return true;
        }

        private static void CheckForConcurrency(int expectedVersion, int lastStoredEventVersion)
        {
            if (lastStoredEventVersion == expectedVersion) return;
            var error = $"Expected: {expectedVersion}. Found: {lastStoredEventVersion}";
            throw new OptimisticConcurrencyException(error);
        }

        public void CreateStream(string eventStreamId, Guid aggregateId, IEnumerable<DomainEvent> domainEvents)
        {
            var eventStream = new EventStream(eventStreamId, aggregateId);
            _eventStreamCollection.InsertOne(eventStream);

            AppendToStream(eventStream.Id, domainEvents, Option.None<int>());
        }

        public Option<IEnumerable<StoredEvent>> GetStoredEvents(string eventStreamId, int afterVersion, int maxCount)
        {
            var eventStream = _eventStreamCollection.Find(x => x.Id == eventStreamId).FirstOrDefault();

            if (eventStream == null)
            {
                return Option.None<IEnumerable<StoredEvent>>();
            }

            var storedEvents = _storedEventCollection
                .Find(x => x.EventStreamId == eventStreamId && x.Version > afterVersion).Limit(maxCount).ToEnumerable();

            return Option.Some(storedEvents);
        }

        public bool AddSnapshot<T>(string eventStreamId, T snapshot)
        {
            var eventStream = _eventStreamCollection.Find(x => x.Id == eventStreamId).FirstOrDefault();

            if (eventStream == null)
            {
                return false;
            }

            var storedSnapshot = new StoredSnapshot(eventStream.Id, snapshot);

            _storedSnapshotCollection.InsertOne(storedSnapshot);

            return true;
        }

        public Option<T> GetLatestSnapshot<T>(string eventStreamId) where T : class
        {
            var sort = new SortDefinitionBuilder<StoredSnapshot>().Descending(l => l.Created);
            var lastStoredSnapshot = _storedSnapshotCollection.Find(x => x.EventStreamId == eventStreamId).Sort(sort)
                .FirstOrDefault();

            if (lastStoredSnapshot == null)
            {
                return Option.None<T>();
            }

            return Option.Some(lastStoredSnapshot.Snapshot as T);
        }
    }
}