using System;
using EventSourcing.Domain.BankAccount;
using NUnit.Framework;

namespace Tests.BankAccountTests
{
    [TestFixture]
    public class BalanceShould
    {
        [Test]
        public void Have_Negative_500_As_OverdraftLimit()
        {            
            Assert.AreEqual(-500.00m, Balance.OverdraftLimit);
        }        
        
        [Test]
        public void Throw_InvalidOperationException_If_Amount_Have_More_Than_Two_Decimal_Points()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var balance = Balance.Create(0.001m);
            });
        }
        
        [Test]
        public void Throw_InvalidOperationException_If_Amount_Is_Lesser_Than_OverdraftLimit()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var balance = Balance.Create(-600m);
            });
        }
        
        [Test]
        public void Throw_InvalidOperationException_If_Amount_To_Withdraw_Is_More_Than_OverdraftLimit()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var balance = Balance.Empty;
                var newBalance = balance.Withdraw(new Money(30000m));
            });
        }

        [Test]
        public void Create_Itself_From_Money()
        {
            var money = new Money(123);
            var balance = Balance.Create(money);
            
            Assert.AreEqual(money.Value, balance.Value);
        }

        [Test]
        public void Create_Itself_From_Decimal_Value()
        {
            var balance = Balance.Create(123m);
            
            Assert.AreEqual(123m, balance.Value);
        }

        [Test]
        public void Deposit_Money()
        {
            var money = new Money(100);
            var balance = Balance.Empty;

            var actual = balance.Deposit(money).Value;
            
            Assert.AreEqual(money.Value, actual);
        }

        [Test]
        public void Withdraw_Money()
        {
            var money = new Money(100m);
            var balance = Balance.Create(300m);
            var newBalance = balance.Withdraw(money);

            var actual = newBalance.Value;

            Assert.AreEqual(balance.Value - money.Value, actual);
        }

        [Test]
        public void Have_Overriden_Equality_Members()
        {
            var balance1 = Balance.Create(123m);
            var balance2 = Balance.Create(123m);
            var balance3 = Balance.Create(122m);
            
            Assert.IsTrue(balance1 == balance2);
            Assert.IsTrue(balance2.Equals(balance1));
            Assert.IsTrue(balance1 != balance3);
            Assert.IsTrue(!balance3.Equals(balance2));
        }

        [Test]
        public void Have_Initialized_Empty_Balance()
        {
            var balance = Balance.Empty;

            var amount = balance.Value;

            Assert.AreEqual(0.00m, amount);
        }
    }
}























