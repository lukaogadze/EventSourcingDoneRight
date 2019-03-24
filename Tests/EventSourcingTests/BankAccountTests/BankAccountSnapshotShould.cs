using System;
using EventSourcing.Domain.BankAccount;
using NUnit.Framework;

namespace Tests.EventSourcingTests.BankAccountTests
{
    [TestFixture]
    public class BankAccountSnapshotShould
    {
        [Test]
        public void Initialize_Identity()
        {
            var snapshot = new BankAccountSnapshot(1, 0, Guid.NewGuid());
            Assert.AreNotEqual(default(Guid), snapshot.Id);
        }

        [Test]
        public void Throw_InvalidOperationException_If_Version_Is_Less_Or_Equal_To_Zero()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var snapshot = new BankAccountSnapshot(0, 0, Guid.NewGuid());
            });
            
            Assert.Throws<InvalidOperationException>(() =>
            {
                var snapshot = new BankAccountSnapshot(-4, 0, Guid.NewGuid());
            });
        }
        
        [Test]
        public void Throw_InvalidOperationException_If_AggregateId_Is_Default_Guid()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var snapshot = new BankAccountSnapshot(1, 0, Guid.Empty);
            });
        }

        [Test]
        public void Initialize_Version()
        {
            var snapshot = new BankAccountSnapshot(1, 0, Guid.NewGuid());
            
            Assert.AreEqual(snapshot.Version, 1);
        }
        
        [Test]
        public void Initialize_Balance()
        {
            var snapshot = new BankAccountSnapshot(1, 0, Guid.NewGuid());
            
            Assert.AreEqual(snapshot.Balance, 0);
        }
        
        [Test]
        public void Initialize_AggregateId()
        {
            var guid = Guid.NewGuid();
            var snapshot = new BankAccountSnapshot(1, 0, guid);
            
            Assert.AreEqual(snapshot.AggregateId, guid);
        }
    }
}