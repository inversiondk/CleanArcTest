using CleanArcTest.Core.Base;
using CleanArcTest.Core.Domain;

namespace CleanArcTest.Core.Events.Trademarks
{
    public class RegistrationAddedToTrademark : DomainEvent
    {
        public RegistrationAddedToTrademark(int trademarkId, Registration registration)
        {
            TrademarkId = trademarkId;
            Registration = registration;
        }

        public int TrademarkId { get; set; }
        public Registration Registration { get; set; }
    }
}
