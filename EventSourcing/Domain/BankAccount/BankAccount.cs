using System;
using System.Collections.Generic;
using EventSourcing.Domain.BankAccount.DomainEvents;

namespace EventSourcing.Domain.BankAccount
{
    public sealed class BankAccount : EventSourcedAggregate
    {
        public Balance Balance { get; private set; }
   
        private BankAccount(Guid id) : base(id)
        {
            Causes(new AccountCreatedWithEmptyBalance());
        }

        private BankAccount(BankAccountSnapshot bankAccountSnapshot, IEnumerable<DomainEvent> storedDomainEvents) : base(bankAccountSnapshot.AggregateId)
        {
            DomainEventVersion = bankAccountSnapshot.Version;
            StoredEventVersion = bankAccountSnapshot.Version;
            Balance = Balance.Create(bankAccountSnapshot.Balance);
            foreach (DomainEvent @event in storedDomainEvents)
            {
                Causes(@event);
                StoredEventVersion++;
            }
        }

        private BankAccount(Guid id, Balance balance) : base(id)
        {
            Causes(new AccountCreatedWithBalance(balance));
        }
        
        public static BankAccount CreateEmptyBankAccount()
        {
            return new BankAccount(Guid.NewGuid());            
        }

        public static BankAccount CreateBankAccountWithBalance(Balance balance)
        {
            return new BankAccount(Guid.NewGuid(), balance);
        }

        public static BankAccount ReconstructBankAccount(BankAccountSnapshot bankAccountSnapshot,
            IEnumerable<DomainEvent> storedDomainEvents)
        {
            return new BankAccount(bankAccountSnapshot, storedDomainEvents);
        }
        
        public BankAccountSnapshot Snapshot() => new BankAccountSnapshot(DomainEventVersion, Balance.Value, Id);  
        
        public void Withdraw(Money money)
        {
            if (!Balance.MoneyCanBeWithdrawn(money))
            {
                // Log or do something
            }
            else
            {
                Causes(new WithdrawnMoney(money));
            }
            
        }

        public void Deposit(Money money)
        {
            Causes(new DepositedMoney(money));
        }
        
        private void Causes(DomainEvent @event)
        {
            Changes.Add(@event);
            Apply(@event);
        }
        
        

        protected override void Apply(DomainEvent @event)
        {
            When((dynamic)@event);
            DomainEventVersion++;            
        }                     
                
        
        private void When(WithdrawnMoney withdrawnMoney)
        {
            Balance = Balance.Withdraw(withdrawnMoney.Amount);
        }
                       

        private void When(DepositedMoney depositedMoney)
        {
            Balance = Balance.Deposit(depositedMoney.Amount);
        }
        
        private void When(AccountCreatedWithEmptyBalance accountCreatedWithEmptyBalance)
        {
            Balance = accountCreatedWithEmptyBalance.Balance;
        }

        private void When(AccountCreatedWithBalance accountCreatedWithBalance)
        {
            Balance = accountCreatedWithBalance.Balance;
        }
    }
}