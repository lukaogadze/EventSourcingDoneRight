using System;
using EventSourcing.Domain.BankAccount;
using Rafaela.Functional;

namespace EventSourcing.Infrastructure.Repository
{
    public interface IBankAccountRepository
    {
        Option<BankAccount> FindBy(Guid id);
        void Add(BankAccount bankAccount);
        bool Save(BankAccount bankAccount);
        bool SaveSnapshot(BankAccount bankAccount);
    }
}