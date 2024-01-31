using YourRest.Domain.Events;

namespace YourRest.Domain.Entities
{
    public abstract class AbstractAggregateRoot : BaseEntity<int>
    {
        private readonly List<IDomainEvent> _events = new List<IDomainEvent>();

        public IReadOnlyList<IDomainEvent> ReleaseEvents()
        {
            var events = new List<IDomainEvent>(_events);
            _events.Clear();
            return events;
        }

        protected void Record(IDomainEvent eventItem)
        {
            _events.Add(eventItem);
        }
    }
}