namespace EventSourcing.Domain.BankAccount.DomainEvents
{
    public class AccountCreatedWithEmptyBalance : DomainEvent
    {
        public Balance Balance { get; protected set; }
        public AccountCreatedWithEmptyBalance()
        {
            Balance = Balance.Empty;
        }
    }
}