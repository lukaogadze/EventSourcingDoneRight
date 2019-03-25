using EventSourcing.Domain.BankAccount;
using EventSourcing.Domain.BankAccount.DomainEvents;
using NUnit.Framework;

namespace Tests.BankAccountTests.DomainEventTests
{
    [TestFixture]
    public class AccountCreatedWithBalanceShould
    {
        [Test]
        public void Initialize_Balance()
        {
            var balance = Balance.Create(123m);
            var accountCreatedWithBalance = new AccountCreatedWithBalance(balance);

            var actual = accountCreatedWithBalance.Balance;
            
            Assert.AreEqual(balance, actual);
        }
    }
}