using System;
using System.Collections.Generic;
using System.Linq;
using EventSourcing.Domain;
using EventSourcing.Domain.BankAccount;
using EventSourcing.Infrastructure.EventStore;
using EventSourcing.Infrastructure.EventStore.Implementations;
using NUnit.Framework;
using Rafaela.Functional;
using Raven.Client.Documents;

namespace Tests.EventSourcingTests.BankAccountTests.ImplementationTests
{
//    [TestFixture]
//    public class RavenDbEventStoreShould
//    {
        
//        [Test]
//        public void Store_And_Reconstruct_With_DomainEvents_And_Id()
//        {
//            using (var store = new DocumentStore
//            {
//                Urls = new[] {"http://raven-db-aglaophonos.c9users.io:8080"},
//                Database = "SampleDataDB"
//            })
//            {
//                store.Initialize();
//
//                using (var session = store.OpenSession())
//                {
//
//                    var bankAccount = GetBankAccount();
//
//                    var eventStore = new RavenDbEventStore(session);
//                    var eventStreamId = GetEventStreamId(typeof(BankAccount), bankAccount.Id);
//                    eventStore.CreateStream(eventStreamId, bankAccount.Id, bankAccount.Changes);
//                    session.SaveChanges();
//
//                    var domainEvents = eventStore.GetStoredEvents(eventStreamId, 0, Int32.MaxValue).Value.Select(x => x.Event);
//                    var reconstructedBankAccount = BankAccount.ReconstructBankAccount(bankAccount.Id, domainEvents);
//                    
//                    
//                    AssertBankAccounts(bankAccount, reconstructedBankAccount);               
//                }
//            }
//        }
//        
//        [Test]
//        public void Store_And_Reconstruct_With_Snapshot()
//        {
//            using (var store = new DocumentStore
//            {
//                Urls = new[] {"http://raven-db-aglaophonos.c9users.io:8080"},
//                Database = "SampleDataDB"
//            })
//            {
//                store.Initialize();
//
//                using (var session = store.OpenSession())
//                {
//
//                    var bankAccount = GetBankAccount();
//                    var snapshot = bankAccount.Snapshot();
//                    
//                    
//
//                    var eventStore = new RavenDbEventStore(session);
//                    var eventStreamId = GetEventStreamId(typeof(BankAccount), bankAccount.Id);
//                    eventStore.AddSnapshot(eventStreamId, snapshot);                    
//                    session.SaveChanges();
//
//
//                    var snapshotFromDb = eventStore.GetLatestSnapshot<BankAccountSnapshot>(eventStreamId).Value;
//                    var reconstructedBankAccount = BankAccount.ReconstructBankAccount(snapshotFromDb, new List<DomainEvent>());
//                    
//                    
//                    AssertBankAccounts(bankAccount, reconstructedBankAccount);                        
//                }
//            }
//        }
//        
//        [Test]
//        public void Throw_OptimisticConcurrencyException()
//        {
//            using (var store = new DocumentStore
//            {
//                Urls = new[] {"http://raven-db-aglaophonos.c9users.io:8080"},
//                Database = "SampleDataDB"
//            })
//            {
//                store.Initialize();
//
//                using (var session = store.OpenSession())
//                {
//
//                    var initialBankAccount = GetBankAccount();
//
//                    var eventStore = new RavenDbEventStore(session);
//                    var eventStreamId = GetEventStreamId(typeof(BankAccount), initialBankAccount.Id);
//                    eventStore.CreateStream(eventStreamId, initialBankAccount.Id, initialBankAccount.Changes);
//                    session.SaveChanges();
//
//                    
//                    
//                    var otherBankAccount = BankAccount.ReconstructBankAccount(initialBankAccount.Snapshot(), new List<DomainEvent>());
//                    otherBankAccount.Withdraw(new Money(500m));
//                    otherBankAccount.Deposit(new Money(500m));
//                    otherBankAccount.Deposit(new Money(500m));
//                                        
//                    eventStore.AppendToStream(eventStreamId, otherBankAccount.Changes, new None<int>());
//                    session.SaveChanges();
//                    
//                    
//                    
//                    
//
//                    Assert.Throws<OptimisticConcurrencyException>(() =>
//                    {
//                        initialBankAccount.Withdraw(new Money(100m));
//                        var lastStoredEventVersion = initialBankAccount.DomainEventVersion;
//                        
//                        eventStore.AppendToStream(eventStreamId, initialBankAccount.Changes, Option.Some(lastStoredEventVersion));
//                        session.SaveChanges();
//                    });
//                }
//            }
//        }
//        
//        
//        [Test]
//        public void Store_And_Reconstruct_With_Snapshot_And_Domain_Events()
//        {
//            using (var store = new DocumentStore
//            {
//                Urls = new[] {"http://raven-db-aglaophonos.c9users.io:8080"},
//                Database = "SampleDataDB"
//            })
//            {
//                store.Initialize();
//
//                using (var session = store.OpenSession())
//                {
//
//                    var bankAccount = GetBankAccount();
//                    var snapshot = bankAccount.Snapshot();
//                    bankAccount.Withdraw(new Money(300m));
//                    
//                    
//
//                    var eventStore = new RavenDbEventStore(session);
//                    var eventStreamId = GetEventStreamId(typeof(BankAccount), bankAccount.Id);
//                    eventStore.AddSnapshot(eventStreamId, snapshot);
//                    eventStore.CreateStream(eventStreamId, bankAccount.Id, bankAccount.Changes);
//                    session.SaveChanges();
//
//
//                    var snapshotFromDb = eventStore.GetLatestSnapshot<BankAccountSnapshot>(eventStreamId).Value;
//                    var domainEventsFromDb = eventStore.GetStoredEvents(eventStreamId, snapshotFromDb.Version, Int32.MaxValue).Value
//                        .Select(x => x.Event);
//                    var reconstructedBankAccount = BankAccount.ReconstructBankAccount(snapshotFromDb, domainEventsFromDb);
//                    
//                    
//                    AssertBankAccounts(bankAccount, reconstructedBankAccount);
//                }
//            }
//        }
//        
//        
//
//        private static void AssertBankAccounts(BankAccount bankAccount, BankAccount reconstructedBankAccount)
//        {
//            Assert.AreEqual(bankAccount.Id, reconstructedBankAccount.Id);
//            Assert.AreEqual(bankAccount.Balance, reconstructedBankAccount.Balance);
//            Assert.AreEqual(0, reconstructedBankAccount.Changes.Count);
//            Assert.AreEqual(bankAccount.DomainEventVersion, reconstructedBankAccount.DomainEventVersion);
//            Assert.AreEqual(bankAccount.DomainEventVersion, reconstructedBankAccount.StoredEventVersion);
//        }
//
//
//        private BankAccount GetBankAccount()
//        {
//            var bankAccount = BankAccount.CreateEmptyBankAccount();
//            bankAccount.Deposit(new Money(100m));
//            bankAccount.Deposit(new Money(100m));
//            bankAccount.Deposit(new Money(100m));
//            bankAccount.Deposit(new Money(100m));
//
//            return bankAccount;
//        }
//        
//        private string GetEventStreamId(Type type, Guid aggregateId)
//        {
//            return $"{type.Name}-{aggregateId}";
//        }
//    }
}