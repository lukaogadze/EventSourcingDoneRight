using System;
using EventSourcing.Domain.BankAccount;
using Rafaela.Functional;

namespace EventSourcing.Infrastructure.Repository
{
    public interface IBankAccountRepository
    {
        Option<BankAccount> FindBy(Guid id);
        void Add(BankAccount bankAccount);
        void Save(BankAccount bankAccount);
        void SaveSnapshot(BankAccount bankAccount);
    }
}