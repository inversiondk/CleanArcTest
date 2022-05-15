using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArcTest.Application.Registrations.Queries.DTOs;
using CleanArcTest.Core.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArcTest.Application.Registrations.Queries
{
    public class GetRegistrationsQuery : IRequest<List<RegistrationDto>>
    {
        public int TrademarkId { get; set; }

        public GetRegistrationsQuery(int trademarkId)
        {
            TrademarkId = trademarkId;
        }
    }

    public class GetRegistrationsQueryHandler : IRequestHandler<GetRegistrationsQuery, List<RegistrationDto>>
    {
        private readonly IApplicationDataContext _context;
        private readonly IMapper _mapper;

        public GetRegistrationsQueryHandler(IApplicationDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<RegistrationDto>> Handle(GetRegistrationsQuery request, CancellationToken cancellationToken)
        {
            // Using EF queries (without repository pattern) with projection via AutoMapper
            return await _context.Registrations
                .Where(i => i.TrademarkId == request.TrademarkId)
                .ProjectTo<RegistrationDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
