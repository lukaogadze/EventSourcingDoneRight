using System;
using System.Linq;
using EventSourcing.Domain.BankAccount;
using EventSourcing.Infrastructure.EventStore;
using Rafaela.Functional;

namespace EventSourcing.Infrastructure.Repository
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly IAppendOnlyStore _appendOnlyStore;

        public BankAccountRepository(IAppendOnlyStore appendOnlyStore)
        {
            _appendOnlyStore = appendOnlyStore;
        }
        
        public Option<BankAccount> FindBy(Guid id)
        {
            string eventStreamId = GetEventStreamId(id);            

            int afterVersion = 0;
            const int eventCount = int.MaxValue;

            Option<BankAccountSnapshot> latestSnapshotOption = _appendOnlyStore.GetLatestSnapshot<BankAccountSnapshot>(eventStreamId);
            if (latestSnapshotOption.IsSome)
            {
                afterVersion = latestSnapshotOption.Value.Version;
            }

            var storedEventsOption = _appendOnlyStore.GetStoredEvents(eventStreamId, afterVersion, eventCount);
            if (storedEventsOption.IsNone)
            {
                return Option.None<BankAccount>();
            }
            

            if (latestSnapshotOption.IsSome && storedEventsOption.IsSome)
            {
                var snapshot = latestSnapshotOption.Value;
                var domainEvents = storedEventsOption.Value.Select(x => x.Event);
                var bankAccount = BankAccount.ReconstructBankAccount(snapshot, domainEvents);
                
                return Option.Some(bankAccount);
            }
            else
            {
                var domainEvents = storedEventsOption.Value.Select(x => x.Event);
                var bankAccount = BankAccount.ReconstructBankAccount(id, domainEvents);
                
                return Option.Some(bankAccount);
            }
        }

        public void Add(BankAccount bankAccount)
        {
            var eventStreamId = GetEventStreamId(bankAccount.Id);
            _appendOnlyStore.CreateStream(eventStreamId, bankAccount.Id, bankAccount.Changes);
        }
        
        

        public void Save(BankAccount bankAccount)
        {
            var eventStreamId = GetEventStreamId(bankAccount.Id);

            var expectedVersion = bankAccount.StoredEventVersion == 0 
                ? Option.None<int>() 
                : Option.Some(bankAccount.StoredEventVersion);
            
            _appendOnlyStore.AppendToStream(eventStreamId, bankAccount.Changes, expectedVersion);
        }

        public void SaveSnapshot(BankAccount bankAccount)
        {
            var eventStreamId = GetEventStreamId(bankAccount.Id);
            _appendOnlyStore.AddSnapshot(eventStreamId, bankAccount.Snapshot());
        }
        
        
        private string GetEventStreamId(Guid bankAccountId)
        {
            return $"{nameof(BankAccount)}-{bankAccountId}";
        }
    }
}