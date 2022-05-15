using AutoMapper;
using CleanArcTest.Application.Trademarks.Queries.DTOs;
using CleanArcTest.Core.Repositories.Query;
using MediatR;

namespace CleanArcTest.Application.Trademarks.Queries
{
    public class GetTrademarksQuery : IRequest<List<TrademarkDto>>
    { }

    public class GetTrademarksQueryHandler : IRequestHandler<GetTrademarksQuery, List<TrademarkDto>>
    {
        private readonly ITrademarkQueryRepository _trademarkQueryRepository;
        private readonly IMapper _mapper;

        public GetTrademarksQueryHandler(ITrademarkQueryRepository trademarkQueryRepository, IMapper mapper)
        {
            _trademarkQueryRepository = trademarkQueryRepository;
            _mapper = mapper;
        }

        public async Task<List<TrademarkDto>> Handle(GetTrademarksQuery request, CancellationToken cancellationToken)
        {
            // Using readonly repository (Dapper)
            var data = await _trademarkQueryRepository.GetAllTrademarksAsync(cancellationToken);
            return _mapper.Map<List<TrademarkDto>>(data).ToList();
        }
        
    }
}
