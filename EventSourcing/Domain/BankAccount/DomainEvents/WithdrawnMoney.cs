namespace EventSourcing.Domain.BankAccount.DomainEvents
{
    public class WithdrawnMoney : DomainEvent
    {
        public Money Amount { get; protected set; }

        public WithdrawnMoney(Money amount)
        {
            Amount = amount;
        }
    }
}