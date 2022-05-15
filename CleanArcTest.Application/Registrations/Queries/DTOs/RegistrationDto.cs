using AutoMapper;
using CleanArcTest.Application.Common.Mappings;
using CleanArcTest.Core.Domain;

namespace CleanArcTest.Application.Registrations.Queries.DTOs
{
    public class RegistrationDto : IMapFrom<Registration>
    {
        public string TrademarkName { get; set; }
        public string CountryIso { get; set; }
        public decimal RenewalPrice { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Registration, RegistrationDto>()
                .ForMember(d => d.CountryIso, opt => opt.MapFrom(s => s.CountryIso))
                .ForMember(d => d.RenewalPrice, opt => opt.MapFrom(s => s.RenewalPrice))
                .ForMember(d => d.TrademarkName, opt => opt.MapFrom(s => s.Trademark.Name));
        }

    }
}
