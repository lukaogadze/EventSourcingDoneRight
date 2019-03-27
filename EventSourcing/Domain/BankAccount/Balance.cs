using System;
using System.Collections.Generic;
using Rafaela.DDD;

namespace EventSourcing.Domain.BankAccount
{
    public class Balance : ValueObject
    {
        public static Balance Empty => new Balance(0.00m);
        public const decimal OverdraftLimit = -500.00m;
        public decimal Value { get; protected set; }

        private Balance(decimal amount)
        {
            Value = amount;
        }
        
        public static Balance Create(decimal amount)
        {
            if (amount % 0.01m != 0)
                throw new InvalidOperationException("There cannot be more than two decimal places."); 

            if (amount < OverdraftLimit)
            {
                throw new InvalidOperationException("Amount cannot be lesser than Overdraft Limit");
            }            
            
            return new Balance(amount);
        }
        
        public static Balance Create(Money amount)
        {
            return new Balance(amount.Value);
        } 

        public Balance Deposit(Money amount)
        {
            return new Balance(amount.Value + Value);
        }               

        public Balance Withdraw(Money amount)
        {
            
            if (MoneyCanBeWithdrawn(amount))
            {
                var amountLeft = Value - amount.Value;
                return new Balance(amountLeft);
            }
            throw new InvalidOperationException("exceed overdraft limit");
        }

        public bool MoneyCanBeWithdrawn(Money money)
        {
            var amountLeft = Value - money.Value;
            return amountLeft >= OverdraftLimit;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}