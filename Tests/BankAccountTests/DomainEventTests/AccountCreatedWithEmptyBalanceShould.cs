using EventSourcing.Domain.BankAccount;
using EventSourcing.Domain.BankAccount.DomainEvents;
using NUnit.Framework;

namespace Tests.BankAccountTests.DomainEventTests
{
    [TestFixture]
    public class AccountCreatedWithEmptyBalanceShould
    {
        [Test]
        public void Initialize_Balance_With_Empty_Balance()
        {
            var accountCreatedWithEmptyBalance = new AccountCreatedWithEmptyBalance();
            
            Assert.AreEqual(Balance.Empty, accountCreatedWithEmptyBalance.Balance);
        }
    }
}