using System;
using EventSourcing.Infrastructure.EventStore;
using NUnit.Framework;

namespace Tests.EventSourcingTests.InfrastructureTests
{
    [TestFixture]
    public class StoredSnapshotShould
    {
        [Test]
        public void Initialize_Identity()
        {
            var eventStreamId = Guid.NewGuid().ToString();
            var storedSnapshot = new StoredSnapshot(eventStreamId, new object());
            
            Assert.IsNotNull(storedSnapshot.Id);
        }
        
        [Test]
        public void Initialize_EventStreamId()
        {
            var eventStreamId = Guid.NewGuid().ToString();
            var storedSnapshot = new StoredSnapshot(eventStreamId, new object());
            
            Assert.IsNotNull(storedSnapshot.EventStreamId);
        }
        
        [Test]
        public void Initialize_Snapshot()
        {
            var eventStreamId = Guid.NewGuid().ToString();
            var storedSnapshot = new StoredSnapshot(eventStreamId, new object());
            
            Assert.IsNotNull(storedSnapshot.Snapshot);
        }
        
        [Test]
        public void Initialize_Created()
        {
            var eventStreamId = Guid.NewGuid().ToString();
            var storedSnapshot = new StoredSnapshot(eventStreamId, new object());
            
            Assert.AreNotEqual(default(DateTimeOffset), storedSnapshot.Created);
        }

        [Test]
        public void Throw_ArgumentNullException_On_Initialization_If_EventStreamId_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                string eventStreamId = null;
                var storedSnapshot = new StoredSnapshot(eventStreamId, new object());
            });
        }
        
        [Test]
        public void Throw_ArgumentNullException_On_Initialization_If_Snapshot_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                string eventStreamId = Guid.NewGuid().ToString();
                var storedSnapshot = new StoredSnapshot(eventStreamId, null);
            });
        }
    }
}