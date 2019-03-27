using System;
using System.Linq;
using EventSourcing.Domain.BankAccount;
using EventSourcing.Infrastructure.EventStore;
using EventSourcing.Infrastructure.EventStore.Implementations;
using EventSourcing.Infrastructure.Repository;
using MongoDB.Driver;
using NUnit.Framework;
using Raven.Client.Documents;

namespace Tests.EventSourcingTests.BankAccountTests.ImplementationTests
{
//    [TestFixture]
//    public class BankAccountRepositoryShould
//    {
//        
//        private IAppendOnlyStore _appendOnlyStoreMongo;
//        
//        [SetUp]
//        public void SetUp()
//        {
//            var client = new MongoClient();
//            var eventSourcingDb = client.GetDatabase("eventsourcing");            
//            
//
//            var storedSnapshotCollection = eventSourcingDb.GetCollection<StoredSnapshot>("storedsnapshots");
//            var storedEventCollection = eventSourcingDb.GetCollection<StoredEvent>("storedevents");
//            var eventStreamCollection = eventSourcingDb.GetCollection<EventStream>("eventstreams");                       
//            
//            _appendOnlyStoreMongo = new MongoDbEventStore(eventStreamCollection, storedEventCollection, storedSnapshotCollection);
//        }
//        
//        [TearDown]
//        public void TearDown()
//        {
//            var client = new MongoClient();
//            var eventSourcingDb = client.GetDatabase("eventsourcing");            
//            
//
//            var storedSnapshotCollection = eventSourcingDb.GetCollection<StoredSnapshot>("storedsnapshots");
//            var storedEventCollection = eventSourcingDb.GetCollection<StoredEvent>("storedevents");
//            var eventStreamCollection = eventSourcingDb.GetCollection<EventStream>("eventstreams");
//
//            storedSnapshotCollection.DeleteMany(x => true);
//            storedEventCollection.DeleteMany(x => true);
//            eventStreamCollection.DeleteMany(x => true);
//        }

//        [Test]
//        public void Add_Account_Raven()
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
//                    IAppendOnlyStore appendOnlyStore = new RavenDbEventStore(session);
//                    IBankAccountRepository repository = new BankAccountRepository(appendOnlyStore);                    
//
//                    var bankAccount = GetBankAccount();
//                    repository.Add(bankAccount);
//                    session.SaveChanges();
//
//                    var bankAccountFromDbOption = repository.FindBy(bankAccount.Id);                    
//                    
//                    Assert.IsTrue(bankAccountFromDbOption.IsSome);
//                    AssertBankAccounts(bankAccount, bankAccountFromDbOption.Value);
//                }
//            }
//        }
//        
//        [Test]
//        public void Add_Account_Mongo()
//        {
//            IBankAccountRepository repository = new BankAccountRepository(_appendOnlyStoreMongo);                    
//
//            var bankAccount = GetBankAccountWith400Balance();
//            repository.Add(bankAccount);
//
//            var bankAccountFromDbOption = repository.FindBy(bankAccount.Id);                    
//                    
//            Assert.IsTrue(bankAccountFromDbOption.IsSome);
//            AssertBankAccounts(bankAccount, bankAccountFromDbOption.Value);
//        }
//                        


//        [Test]
//        public void Save_Account_Raven()
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
//                    IAppendOnlyStore appendOnlyStore = new RavenDbEventStore(session);
//                    IBankAccountRepository repository = new BankAccountRepository(appendOnlyStore);                    
//
//                    var bankAccount = GetBankAccount();
//                    repository.Add(bankAccount);
//                    session.SaveChanges();
//
//                    var bankAccountOption = repository.FindBy(bankAccount.Id);
//                    var bankAccountFromDb = bankAccountOption.Value;
//                    
//                    bankAccountFromDb.Deposit(new Money(1000m));
//                    bankAccountFromDb.Deposit(new Money(1000m));
//                    repository.Save(bankAccountFromDb);
//                    session.SaveChanges();
//
//                    var updatedBankAccount = repository.FindBy(bankAccountFromDb.Id);
//                    
//                    Assert.IsTrue(updatedBankAccount.IsSome);
//                    AssertBankAccounts(bankAccountFromDb, updatedBankAccount.Value);
//                }
//            }
//        }
//
//        [Test]
//        public void Save_Account_Mongo()
//        {
//            IBankAccountRepository repository = new BankAccountRepository(_appendOnlyStoreMongo);                    
//
//            var bankAccount = GetBankAccountWith400Balance();
//            repository.Add(bankAccount);
//
//            var bankAccountOption = repository.FindBy(bankAccount.Id);
//            var bankAccountFromDb = bankAccountOption.Value;
//                    
//            bankAccountFromDb.Deposit(new Money(1000m));
//            bankAccountFromDb.Deposit(new Money(1000m));
//            repository.Save(bankAccountFromDb);
//
//            var updatedBankAccount = repository.FindBy(bankAccountFromDb.Id);
//                    
//            Assert.IsTrue(updatedBankAccount.IsSome);
//            AssertBankAccounts(bankAccountFromDb, updatedBankAccount.Value);
//        }
        
        
//        [Test]
//        public void Save_Snapshot_Raven()
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
//                    IAppendOnlyStore appendOnlyStore = new RavenDbEventStore(session);
//                    IBankAccountRepository repository = new BankAccountRepository(appendOnlyStore);                    
//
//                    var bankAccount = GetBankAccount();
//                    repository.Add(bankAccount);
//                    repository.SaveSnapshot(bankAccount);                    
//                    session.SaveChanges();
//                    
//                    bankAccount.Withdraw(new Money(500m));
//                    bankAccount.Deposit(new Money(550m));
//                    repository.Save(bankAccount);
//                    session.SaveChanges();
//                    
//                    var bankAccountOption = repository.FindBy(bankAccount.Id);
//                }
//            }
//        }
//
//        [Test]
//        public void Save_Snapshot_Mongo()
//        {
//            IBankAccountRepository repository = new BankAccountRepository(_appendOnlyStoreMongo);                    
//
//            var bankAccount = GetBankAccountWith400Balance();
//            repository.Add(bankAccount);
//            repository.SaveSnapshot(bankAccount);                    
//                    
//            bankAccount.Withdraw(new Money(500m));
//            bankAccount.Deposit(new Money(550m));
//            repository.Save(bankAccount);
//                    
//            var bankAccountOption = repository.FindBy(bankAccount.Id);
//        }
//        
//        
//        
//        
//       
//        private static void AssertBankAccounts(BankAccount bankAccount, BankAccount bankAccountFromDb)
//        {            
//            Assert.AreEqual(bankAccount.Id, bankAccountFromDb.Id);
//            Assert.AreEqual(bankAccount.Balance, bankAccountFromDb.Balance);
//            Assert.AreEqual(0, bankAccountFromDb.Changes.Count);
//            Assert.AreEqual(bankAccount.DomainEventVersion, bankAccountFromDb.DomainEventVersion);
//            Assert.AreEqual(bankAccount.DomainEventVersion, bankAccountFromDb.StoredEventVersion);
//        }
//        
//
//        private BankAccount GetBankAccountWith400Balance()
//        {
//            var bankAccount = BankAccount.CreateEmptyBankAccount();
//            bankAccount.Deposit(new Money(100m));
//            bankAccount.Deposit(new Money(100m));
//            bankAccount.Deposit(new Money(100m));
//            bankAccount.Deposit(new Money(100m));
//
//            return bankAccount;
//        }
//    }
}