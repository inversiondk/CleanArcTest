using CleanArcTest.Core.Base;
using CleanArcTest.Core.Base.Contracts;

namespace CleanArcTest.Core.Domain
{
    public class Registration : EntityWithDomainEvents, IAggregateRoot
    {
        public int TrademarkId { get; private set; }
        public Trademark Trademark { get; private set; }
        public string CountryIso { get; private set; }
        public decimal RenewalPrice { get; private set; }

        private Registration()
        {
        }

        public Registration(string countryIso, decimal renewalPrice)
        {
            CountryIso = countryIso;
            RenewalPrice = renewalPrice;
        }
    }
}
