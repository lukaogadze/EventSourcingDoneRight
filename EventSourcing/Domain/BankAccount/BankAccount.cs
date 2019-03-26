using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EventSourcing.Domain.BankAccount.DomainEvents;

namespace EventSourcing.Domain.BankAccount
{
    public sealed class BankAccount : EventSourcedAggregate
    {
        public override ReadOnlyCollection<DomainEvent> Changes => new ReadOnlyCollection<DomainEvent>(_changes);
        private readonly List<DomainEvent> _changes = new List<DomainEvent>();
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
                Apply(@event);
                StoredEventVersion++;
            }
        }

        private BankAccount(Guid id, IEnumerable<DomainEvent> storedDomainEvents) : base(id)
        {
            foreach (DomainEvent @event in storedDomainEvents)
            {
                Apply(@event);
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
        
        public static BankAccount ReconstructBankAccount(Guid bankAccountId, IEnumerable<DomainEvent> storedDomainEvents)
        {
            BankAccount bankAccount = new BankAccount(bankAccountId, storedDomainEvents);

            return bankAccount;
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
            _changes.Add(@event);
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