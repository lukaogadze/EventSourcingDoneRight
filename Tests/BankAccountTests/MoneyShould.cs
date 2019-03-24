using System;
using EventSourcing.Domain.BankAccount;
using NUnit.Framework;

namespace Tests.BankAccountTests
{
    [TestFixture]
    public class MoneyShould
    {
        [Test]
        public void Throw_InvalidOperationException_When_Passed_Value_Has_More_Than_Two_Decimal_Points()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var money = new Money(0.0001m);                
            });
        }
        
        [Test]
        public void Throw_InvalidOperationException_When_Passed_Value_Is_Negative()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var money = new Money(-0.1m);                
            });
        }

        [Test]
        public void Assign_Valid_Value_To_Value_Property()
        {
            const decimal amount = 15.05m;
            var money = new Money(amount);

            var actual = money.Value;
            
            Assert.AreEqual(amount, actual);
        }

        [Test]
        public void Have_Overriden_Equality_Members()
        {
            const decimal amount = 15.05m;
            var money1 = new Money(amount);
            var money2 = new Money(amount);
            var money3 = new Money(amount + 0.5m);
            
            Assert.IsTrue(money1 == money2);
            Assert.IsTrue(money1.Equals(money2));                        
            Assert.IsTrue(money3 != money1);
            Assert.IsTrue(!money3.Equals(money1));
        }
    }
}