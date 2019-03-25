using EventSourcing.Domain.BankAccount;
using EventSourcing.Domain.BankAccount.DomainEvents;
using EventSourcing.Infrastructure.EventStore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.EventSourcingTests.InfrastructureTests
{
    [TestFixture]
    public class StoredEventShould
    {
        [Test]
        public void Throw_ArgumentNullException_On_Initialization_If_EventStreamId_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                StoredEvent storedEvent = new StoredEvent(null, null, 0);
            });
        }

        [Test]
        public void Throw_ArgumentNullException_On_Initialization_If_DomainEvent_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                StoredEvent storedEvent = new StoredEvent(Guid.NewGuid().ToString(), null, 1);
            });
        }

        [Test]
        public void Throw_InvalidOperationException_On_Initialization_If_LastStoredEventVersion_Is_Negative_Or_Zero()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                StoredEvent storedEvent = new StoredEvent(Guid.NewGuid().ToString(), new DepositedMoney(new Money(123m)), 0);
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                StoredEvent storedEvent = new StoredEvent(Guid.NewGuid().ToString(), new DepositedMoney(new Money(123m)), -1);
            });
        }

        [Test]
        public void Initialize_Identity()
        {
            var storedEvent = new StoredEvent(Guid.NewGuid().ToString(), new DepositedMoney(new Money(123m)), 1);

            Assert.IsNotNull(storedEvent.Id);
        }

        [Test]
        public void Initialize_DomainEvent()
        {
            var @event = new DepositedMoney(new Money(123m));
            var storedEvent = new StoredEvent(Guid.NewGuid().ToString(), @event, 1);

            Assert.AreEqual(@event, storedEvent.Event);
        }

        [Test]
        public void Initialize_Version()
        {
            var @event = new DepositedMoney(new Money(123m));
            var storedEvent = new StoredEvent(Guid.NewGuid().ToString(), @event, 1);

            Assert.AreEqual(1, storedEvent.Version);
        }

        [Test]
        public void Initialize_EventStreamId()
        {
            var @event = new DepositedMoney(new Money(123m));
            var guid = Guid.NewGuid();
            var storedEvent = new StoredEvent(guid.ToString(), @event, 1);

            Assert.AreEqual(guid.ToString(), storedEvent.EventStreamId);
        }
    }
}
