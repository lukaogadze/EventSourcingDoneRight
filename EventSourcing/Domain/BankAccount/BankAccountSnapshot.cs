using System;

namespace EventSourcing.Domain.BankAccount
{
    public class BankAccountSnapshot
    {
        public Guid Id { get; protected set; }
        public int Version { get; protected set;}
        public decimal Balance { get; protected set;}
        public Guid AggregateId { get; protected set; }

        public BankAccountSnapshot(int version, decimal balance, Guid aggregateId)
        {
            Id = Guid.NewGuid();
            if (version <= 0)
            {
                throw new InvalidOperationException("Version should be greater than zero");
            }            
            Version = version;
            Balance = balance;

            if (default(Guid) == aggregateId)
            {
                throw new InvalidOperationException("AggregateId should be initialized");
            }
            AggregateId = aggregateId;
        }
    }
}