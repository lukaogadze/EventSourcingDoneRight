using System;

namespace EventSourcing.Infrastructure.EventStore
{
    public class OptimisticConcurrencyException : Exception
    {
        public OptimisticConcurrencyException(string errorMessage) : base(errorMessage)
        {
            
        }
    }
}