namespace CleanArcTest.Api.Models
{
    public class AddRegistrationModel
    {
        public AddRegistrationModel(int trademarkId, string countryIso, decimal renewalPrice)
        {
            TrademarkId = trademarkId;
            CountryIso = countryIso;
            RenewalPrice = renewalPrice;
        }

        public int TrademarkId { get; set; }
        public string CountryIso { get; set; }
        public decimal RenewalPrice { get; set; }
    }
}
