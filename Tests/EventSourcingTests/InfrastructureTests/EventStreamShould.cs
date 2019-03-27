using EventSourcing.Domain.BankAccount;
using EventSourcing.Domain.BankAccount.DomainEvents;
using EventSourcing.Infrastructure.EventStore;
using NUnit.Framework;
using System;

namespace Tests.EventSourcingTests.InfrastructureTests
{
    [TestFixture]
    public class EventStreamShould
    {
        [Test]
        public void Throw_ArgumentNullException_On_Initialization_If_EventStreamId_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var eventStream = new EventStream(null, Guid.Empty);
            });
        }
        
        [Test]
        public void Initialize_Identity()
        {            
            var eventStream = new EventStream("", Guid.NewGuid());

            Assert.IsNotNull(eventStream.Id);
        }

        [Test]
        public void Throw_InvalidOperationException_On_Initialization_If_AggregateId_Is_Default_Value()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var eventStream = new EventStream("",Guid.Empty);
            });
        }

        [Test]
        public void Initialize_AggregateId()
        {
            var eventStream = new EventStream("", Guid.NewGuid());

            Assert.AreNotEqual(default(Guid), eventStream.AggregateId);
        }

        [Test]
        public void Initialize_LastStoredEventVersion()
        {
            var eventStream = new EventStream("", Guid.NewGuid());

            Assert.AreEqual(0, eventStream.LastStoredEventVersion);
        }


        [Test]
        public void Throw_ArgumentNullException_On_RegisterEvent_If_DomainEvent_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var eventStream = new EventStream("", Guid.NewGuid());
                var storedEvent = eventStream.RegisterStoredEvent(null);
            });
        }

        [Test]
        public void RegisterEvent()
        {
            var eventStream = new EventStream("", Guid.NewGuid());
            var storedEvent = eventStream.RegisterStoredEvent(new DepositedMoney(new Money(10m)));

            Assert.IsNotNull(storedEvent);
            Assert.AreEqual(1, eventStream.LastStoredEventVersion);
        }
    }
}
