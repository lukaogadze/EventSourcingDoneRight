namespace EventSourcing.Domain.BankAccount.DomainEvents
{
    public class AccountCreatedWithEmptyBalance : DomainEvent
    {
        public Balance Balance { get; }
        public AccountCreatedWithEmptyBalance()
        {
            Balance = Balance.Empty;
        }
    }
}