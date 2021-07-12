using System.Collections.Generic;
using System.Linq;

namespace WisdomPetMedicine.Common
{
    public abstract class AggregateRoot
    {
        private readonly List<IDomainEvent> changes;

        public int Version { get; private set; }

        protected AggregateRoot()
        {
            changes = new List<IDomainEvent>();
        }

        public IEnumerable<IDomainEvent> GetChanges()
        {
            return changes.AsEnumerable();
        }
        public void ClearChanges()
        {
            changes.Clear();
        }
        protected void ApplyDomainEvent(IDomainEvent domainEvent)
        {
            ChangeStateByUsingDomainEvent(domainEvent);
            ValidateState();
            changes.Add(domainEvent);
            Version++;
        }

        public void Load(IEnumerable<IDomainEvent> history)
        {
            foreach (var e in history)
            {
                ApplyDomainEvent(e);
            }
            ClearChanges();
        }

        protected abstract void ChangeStateByUsingDomainEvent(IDomainEvent domainEvent);
        protected abstract void ValidateState();
    }
}