using EventSourcing.Domain.BankAccount;
using EventSourcing.Domain.BankAccount.DomainEvents;
using NUnit.Framework;

namespace Tests.BankAccountTests.DomainEventTests
{
    [TestFixture]
    public class WithdrawnMoneyShould
    {
        [Test]
        public void Initialize_Amount()
        {
            var money = new Money(123);
            var withdrawnMoney = new WithdrawnMoney(money);

            var actual = withdrawnMoney.Amount;

            Assert.AreEqual(money, actual);
        }
    }
}