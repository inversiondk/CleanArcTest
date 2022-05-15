namespace CleanArcTest.Core.Base
{
    public abstract class DomainEvent
    {
        public bool HasBeenPublished { get; set; }
        public DateTimeOffset Occurred { get; protected set; }

        protected DomainEvent()
        {
            Occurred = DateTimeOffset.UtcNow;
        }
    }
}
