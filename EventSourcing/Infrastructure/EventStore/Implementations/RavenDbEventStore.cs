using System;
using System.Collections.Generic;
using System.Linq;
using EventSourcing.Domain;
using Rafaela.Functional;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace EventSourcing.Infrastructure.EventStore.Implementations
{
    public class RavenDbEventStore  : IAppendOnlyStore
    {
        private readonly IDocumentSession _documentSession;

        public RavenDbEventStore(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }
        public void AppendToStream(string eventStreamId, IEnumerable<DomainEvent> domainEvents, Option<int> expectedVersion)
        {
            var stream = _documentSession.Load<EventStream>(eventStreamId);

            if (expectedVersion.IsSome)
            {
                CheckForConcurrency(expectedVersion.Value, stream.LastStoredEventVersion);
            }

            foreach (DomainEvent @event in domainEvents)
            {
                var storedEvent = stream.RegisterStoredEvent(@event);
                _documentSession.Store(storedEvent);
            }
        }

        private void CheckForConcurrency(int expectedVersion, int lastStoredEventVersion)
        {
            if (lastStoredEventVersion == expectedVersion) return;
            var error = $"Expected: {expectedVersion}. Found: {lastStoredEventVersion}";
            throw new OptimisticConcurrencyException(error);

        }

        
        public void CreateStream(string eventStreamId, Guid aggregateId, IEnumerable<DomainEvent> domainEvents)
        {
            var eventStream = new EventStream(eventStreamId, aggregateId);
            _documentSession.Store(eventStream, eventStream.Id.ToString());
            _documentSession.SaveChanges();
            
            AppendToStream(eventStream.Id, domainEvents, Option.None<int>());
        }

        public Option<IEnumerable<StoredEvent>> GetStoredEvents(string eventStreamId, int afterVersion, int maxCount)
        {
            var stream = _documentSession
                .Query<EventStream>()
                .Customize(x => x.WaitForNonStaleResults())
                .FirstOrDefault(x => x.Id == eventStreamId);

            if (stream == null)
            {
                return Option.None<IEnumerable<StoredEvent>>();
            }

            var storedEvents = _documentSession.Query<StoredEvent>()
                .Customize(x => x.WaitForNonStaleResults())
                .Where(x => x.EventStreamId == eventStreamId && x.Version > afterVersion)
                .Take(maxCount)
                .AsEnumerable();                

            return Option.Some(storedEvents);
        }

        public void AddSnapshot<T>(string eventStreamId, T snapshot)
        {
            var storedSnapshot = new StoredSnapshot(eventStreamId, snapshot);
            _documentSession.Store(storedSnapshot, storedSnapshot.Id.ToString());
        }

        public Option<T> GetLatestSnapshot<T>(string eventStreamId) where T : class
        {
            var lastStoredSnapshot = _documentSession.Query<StoredSnapshot>()
                .Customize(x => x.WaitForNonStaleResults())
                .Where(x => x.EventStreamId == eventStreamId)
                .OrderByDescending(x => x.Created)
                .FirstOrDefault();
            
            if (lastStoredSnapshot == null)
            {
                return Option.None<T>();
            }

            return Option.Some(lastStoredSnapshot.Snapshot as T);
        }
    }
}