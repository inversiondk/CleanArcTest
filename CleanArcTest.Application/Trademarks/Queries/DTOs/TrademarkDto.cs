using CleanArcTest.Application.Common.Mappings;
using CleanArcTest.Core.Domain;

namespace CleanArcTest.Application.Trademarks.Queries.DTOs
{
    public class TrademarkDto : IMapFrom<Trademark>
    {
        public string Name { get; set; }
    }
}
