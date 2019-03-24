using System;

namespace EventSourcing.Infrastructure.EventStore.Implementations
{
    public class OptimisticConcurrencyException : Exception
    {
        public OptimisticConcurrencyException(string errorMessage) : base(errorMessage)
        {
            
        }
    }
}