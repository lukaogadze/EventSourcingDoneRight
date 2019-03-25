using EventSourcing.Domain.BankAccount;
using EventSourcing.Domain.BankAccount.DomainEvents;
using NUnit.Framework;

namespace Tests.BankAccountTests.DomainEventTests
{
    [TestFixture]
    public class DepositedMoneyShould
    {
        [Test]
        public void Initialize_Amount()
        {
            var money = new Money(123);
            var depositedMoney = new DepositedMoney(money);

            var actual = depositedMoney.Amount;
            
            Assert.AreEqual(money, actual);
        }
    }
}