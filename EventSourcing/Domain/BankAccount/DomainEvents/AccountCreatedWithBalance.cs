namespace EventSourcing.Domain.BankAccount.DomainEvents
{
    public class AccountCreatedWithBalance : DomainEvent
    {
        public Balance Balance { get; protected set; }

        public AccountCreatedWithBalance(Balance balance)
        {
            Balance = balance;
        }
    }
}