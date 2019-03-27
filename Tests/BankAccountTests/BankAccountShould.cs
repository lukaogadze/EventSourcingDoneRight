using System;
using System.Collections.Generic;
using EventSourcing.Domain;
using EventSourcing.Domain.BankAccount;
using NUnit.Framework;

namespace Tests.BankAccountTests
{
    [TestFixture]
    public class BankAccountShould
    {
        [Test]
        public void Initialize_Identity()
        {
            BankAccount bankAccount1 = BankAccount.CreateEmptyBankAccount();
            BankAccount bankAccount2 = BankAccount.CreateBankAccountWithBalance(Balance.Create(123));
            BankAccount bankAccount3 =
                BankAccount.ReconstructBankAccount(new BankAccountSnapshot(1, 23, Guid.NewGuid()),
                    new List<DomainEvent>());

            var actual1 = bankAccount1.Id;
            var actual2 = bankAccount2.Id;
            var actual3 = bankAccount3.Id;

            Assert.AreNotEqual(default(Guid), actual1);
            Assert.AreNotEqual(default(Guid), actual2);
            Assert.AreNotEqual(default(Guid), actual3);
        }
        
        [Test]
        public void Create_Account_With_Empty_Balance()
        {
            BankAccount bankAccount = BankAccount.CreateEmptyBankAccount();

            var actualBalance = bankAccount.Balance;

            Assert.AreEqual(Balance.Empty, actualBalance);
        }
        
        [Test]
        public void Create_Account_With_Balance()
        {
            BankAccount bankAccount = BankAccount.CreateBankAccountWithBalance(Balance.Create(123));

            var actualBalance = bankAccount.Balance;

            Assert.AreEqual(Balance.Create(123), actualBalance);
        }

        [Test]
        public void Not_Allow_To_Withdraw_When_It_Exceeds_OverdraftLimit()
        {
            BankAccount bankAccount = BankAccount.CreateEmptyBankAccount();
            var money = new Money(600m);
            bankAccount.Withdraw(money);

            var actualBalance = bankAccount.Balance;
            
            Assert.AreEqual(Balance.Empty, actualBalance);

        }

        [Test]
        public void Deposit_Money()
        {
            BankAccount bankAccount = BankAccount.CreateEmptyBankAccount();
            var money = new Money(123); 
            bankAccount.Deposit(money);

            var actualBalance = bankAccount.Balance;

            Assert.AreEqual(Balance.Create(money), actualBalance);
        }
        
                
        [Test]
        public void Withdraw_Money()
        {
            BankAccount bankAccount = BankAccount.CreateEmptyBankAccount();
            var money = new Money(123); 
            bankAccount.Withdraw(money);            

            var actualBalance = bankAccount.Balance;

            Assert.AreEqual(Balance.Create(-123), actualBalance);
        }
    }
}

















