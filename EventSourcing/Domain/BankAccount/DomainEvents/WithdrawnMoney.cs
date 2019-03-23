namespace EventSourcing.Domain.BankAccount.DomainEvents
{
    public class WithdrawnMoney : DomainEvent
    {
        public Money Amount { get; }

        public WithdrawnMoney(Money amount)
        {
            Amount = amount;
        }
    }
}