using System;
using EventSourcing.Domain.BankAccount;
using EventSourcing.Domain.BankAccount.DomainEvents;
using NUnit.Framework;

namespace Tests.BankAccountTests.DomainEventTests
{
    [TestFixture]
    public class AllDomainEventsShould
    {
        
        [Test]
        public void Have_Initialized_OccurredOn()
        {
            var utcNewStart = DateTimeOffset.UtcNow.AddMilliseconds(-1);
            var occurredOns = GetOccurredOns();
            var utcNewEnd = DateTimeOffset.UtcNow.AddMilliseconds(1);
            foreach (DateTimeOffset occurredOn in occurredOns)
            {
                Assert.IsTrue(CheckOccurredOn(utcNewStart, occurredOn, utcNewEnd));
            }
        }
        
        private DateTimeOffset[] GetOccurredOns()
        {           
            var accountCreatedWithBalance = new AccountCreatedWithBalance(Balance.Create(123m));
            var accountCreatedWithEmptyBalance = new AccountCreatedWithEmptyBalance();
            var depositedMoney = new DepositedMoney(new Money(123m));
            var withdrawnMoney = new WithdrawnMoney(new Money(123m));
            
            return new[] {accountCreatedWithBalance.OccurredOn, accountCreatedWithEmptyBalance.OccurredOn, depositedMoney.OccurredOn, withdrawnMoney.OccurredOn};
        }

        private bool CheckOccurredOn(DateTimeOffset start, DateTimeOffset occurredOn, DateTimeOffset end)
        {
            return start.ToUnixTimeMilliseconds() < occurredOn.ToUnixTimeMilliseconds() &&
                   occurredOn.ToUnixTimeMilliseconds() < end.ToUnixTimeMilliseconds();
        }
    }
}