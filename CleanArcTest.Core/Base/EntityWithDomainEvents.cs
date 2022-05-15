namespace CleanArcTest.Core.Base
{
    /// <summary>
    /// Entity with domain events to be published upon persistence 
    /// </summary>
    public abstract class EntityWithDomainEvents : Entity
    {
        private readonly List<DomainEvent> _domainEvents;
        public IReadOnlyCollection<DomainEvent> DomainEvents
        {
            get => _domainEvents;
            private init => _domainEvents = (List<DomainEvent>)value;
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
        protected void AddDomainEvent(DomainEvent @event)
        {
            _domainEvents.Add(@event);
        }

        protected EntityWithDomainEvents()
        {
            DomainEvents = new List<DomainEvent>();
        }
    }
}
