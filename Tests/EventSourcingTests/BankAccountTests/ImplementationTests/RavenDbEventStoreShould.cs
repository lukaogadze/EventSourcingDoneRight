using EventSourcing.Domain.BankAccount;
using EventSourcing.Infrastructure.EventStore.Implementations;
using NUnit.Framework;
using Raven.Client.Documents;

namespace Tests.EventSourcingTests.BankAccountTests.ImplementationTests
{
    [TestFixture]
    public class RavenDbEventStoreShould
    {
        [Test]
        public void Sample()
        {
            using (var store = new DocumentStore
            {
                Urls = new string[] {"https://raven-db-aglaophonos.c9users.io:8080"},
                Database = "SampleDataDB"
            })
            {
                store.Initialize();

                using (var session = store.OpenSession())
                {
                    var eventStore = new RavenDbEventStore(session);
                    var bankAccount = BankAccount.CreateEmptyBankAccount();
                    bankAccount.Deposit(new Money(100m));
                    bankAccount.Deposit(new Money(100m));
                    bankAccount.Withdraw(new Money(300m));
                    eventStore.Create(bankAccount.Id, bankAccount.Changes);
                    session.SaveChanges();
                    

                }
            }
        }
    }
}