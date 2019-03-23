namespace EventSourcing.Domain.BankAccount.DomainEvents
{
    public class AccountCreatedWithBalance : DomainEvent
    {
        public Balance Balance { get; }

        public AccountCreatedWithBalance(Balance balance)
        {
            Balance = balance;
        }
    }
}