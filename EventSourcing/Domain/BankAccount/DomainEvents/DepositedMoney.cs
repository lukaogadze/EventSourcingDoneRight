namespace EventSourcing.Domain.BankAccount.DomainEvents
{
    public class DepositedMoney : DomainEvent
    {
        public Money Amount { get; }

        public DepositedMoney(Money amount)
        {
            Amount = amount;
        }
    }
}