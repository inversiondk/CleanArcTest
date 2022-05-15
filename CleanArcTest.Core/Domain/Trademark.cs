using CleanArcTest.Core.Base;
using CleanArcTest.Core.Base.Contracts;
using CleanArcTest.Core.Events.Trademarks;

namespace CleanArcTest.Core.Domain
{
    public class Trademark : EntityWithDomainEvents, IAggregateRoot
    {
        private Trademark()
        {
            // Needed for EF
            _registrations = new List<Registration>();
        }

        public Trademark(string name)
        {
            Name = name;
            _registrations = new List<Registration>();
        }

        public string Name { get; private set; }

        private List<Registration> _registrations;
        public IReadOnlyCollection<Registration> Registrations
        {
            get => _registrations;
            private set => _registrations = (List<Registration>)value;
        }

        public void ChangeName(string newName)
        {
            var oldName = Name;
            Name = newName;

            AddDomainEvent(new TrademarkNameChanged(Id, oldName, newName));
        }

        public void AddRegistration(Registration registration)
        {
            _registrations.Add(registration);
            AddDomainEvent(new RegistrationAddedToTrademark(Id, registration));
        }
    }
}
