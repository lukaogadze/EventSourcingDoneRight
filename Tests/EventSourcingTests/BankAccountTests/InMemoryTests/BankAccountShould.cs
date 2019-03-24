using System.Collections.Generic;
using EventSourcing.Domain;
using EventSourcing.Domain.BankAccount;
using EventSourcing.Domain.BankAccount.DomainEvents;
using NUnit.Framework;

namespace Tests.EventSourcingTests.BankAccountTests.InMemoryTests
{
    [TestFixture]
    public class BankAccountShould
    {
        [Test]
        public void Have_Zero_As_StoredEventVersion_After_Initialization()
        {
            EventSourcing.Domain.BankAccount.BankAccount bankAccount1 = EventSourcing.Domain.BankAccount.BankAccount.CreateEmptyBankAccount();
            EventSourcing.Domain.BankAccount.BankAccount bankAccount2 = EventSourcing.Domain.BankAccount.BankAccount.CreateBankAccountWithBalance(Balance.Create(123));

            var actual1 = bankAccount1.StoredEventVersion;
            var actual2 = bankAccount2.StoredEventVersion;

            Assert.AreEqual(default(long), actual1);
            Assert.AreEqual(default(long), actual2);
        }

        [Test]
        public void Store_Event_For_Every_Mutation()
        {
            EventSourcing.Domain.BankAccount.BankAccount bankAccountWithBalance = EventSourcing.Domain.BankAccount.BankAccount.CreateBankAccountWithBalance(Balance.Create(123));
            EventSourcing.Domain.BankAccount.BankAccount bankAccount = EventSourcing.Domain.BankAccount.BankAccount.CreateEmptyBankAccount();
            bankAccount.Deposit(new Money(1));
            bankAccount.Withdraw(new Money(3));

            var actualBankAccountWithBalanceEvent = bankAccountWithBalance.Changes[0];
            var actualEmptyBankAccountEvent = bankAccount.Changes[0];
            var actualDepositEvent = bankAccount.Changes[1];
            var actualWithdrawEvent = bankAccount.Changes[2];

            Assert.IsInstanceOf<AccountCreatedWithBalance>(actualBankAccountWithBalanceEvent);
            Assert.IsInstanceOf<AccountCreatedWithEmptyBalance>(actualEmptyBankAccountEvent);
            Assert.IsInstanceOf<DepositedMoney>(actualDepositEvent);
            Assert.IsInstanceOf<WithdrawnMoney>(actualWithdrawEvent);
        }

        [Test]
        public void Update_DomainEventVersion_On_Every_Mutation()
        {
            EventSourcing.Domain.BankAccount.BankAccount bankAccount1 = EventSourcing.Domain.BankAccount.BankAccount.CreateBankAccountWithBalance(Balance.Create(123));
            EventSourcing.Domain.BankAccount.BankAccount bankAccount2 = EventSourcing.Domain.BankAccount.BankAccount.CreateEmptyBankAccount();
            bankAccount2.Deposit(new Money(1));
            bankAccount2.Withdraw(new Money(3));

            var actual1 = bankAccount1.DomainEventVersion;
            var actual2 = bankAccount2.DomainEventVersion;

            Assert.AreEqual(1, actual1);
            Assert.AreEqual(3, actual2);
        }
        

        [Test]
        public void Reconstruct_Itself_From_Snapshot_And_Empty_List_Of_Domain_Events()
        {
            var bankAccount = EventSourcing.Domain.BankAccount.BankAccount.CreateEmptyBankAccount();
            bankAccount.Deposit(new Money(100));
            bankAccount.Deposit(new Money(100));
            bankAccount.Deposit(new Money(100));
                        

            var snapshot = bankAccount.Snapshot();

            var reconstructedAccount = EventSourcing.Domain.BankAccount.BankAccount.ReconstructBankAccount(snapshot, new List<DomainEvent>());

            Assert.AreEqual(bankAccount.Id, reconstructedAccount.Id);
            Assert.AreEqual(bankAccount.Balance, reconstructedAccount.Balance);
            Assert.AreEqual(bankAccount.DomainEventVersion, reconstructedAccount.DomainEventVersion);
            Assert.AreEqual(bankAccount.DomainEventVersion, reconstructedAccount.StoredEventVersion);
            Assert.AreEqual(0, reconstructedAccount.Changes.Count);
        }
        
        [Test]
        public void Reconstruct_Itself_From_Snapshot_And_Non_Empty_List_Of_Domain_Events()
        {
            var bankAccount = EventSourcing.Domain.BankAccount.BankAccount.CreateEmptyBankAccount();
            bankAccount.Deposit(new Money(100));
            bankAccount.Deposit(new Money(100));
            bankAccount.Deposit(new Money(100));
                        

            var snapshot = bankAccount.Snapshot();

            var reconstructedAccount = EventSourcing.Domain.BankAccount.BankAccount.ReconstructBankAccount(snapshot, new List<DomainEvent>
            {
                new WithdrawnMoney(new Money(100)),
                new WithdrawnMoney(new Money(100))
            });            
            
            Assert.AreEqual(Balance.Create(new Money(100)), reconstructedAccount.Balance);
            Assert.AreEqual(0, reconstructedAccount.Changes.Count);
            Assert.AreEqual(reconstructedAccount.DomainEventVersion, reconstructedAccount.StoredEventVersion);
        }
    }
}

















