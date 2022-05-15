using CleanArcTest.Core.Base.Contracts;
using CleanArcTest.Core.Domain;
using MediatR;

namespace CleanArcTest.Application.Trademarks.Commands.AddRegistration
{
    public class AddRegistrationCommand : IRequest<bool>
    {
        public int TrademarkId { get; set; }
        public string CountryIso { get; set; }
        public decimal RenewalPrice { get; set; }

        public AddRegistrationCommand(int trademarkId, string countryIso, decimal renewalPrice)
        {
            TrademarkId = trademarkId;
            CountryIso = countryIso;
            RenewalPrice = renewalPrice;
        }
    }

    public class AddRegistrationCommandHandler : IRequestHandler<AddRegistrationCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;


        public AddRegistrationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(AddRegistrationCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Trademarks.AddRegistration(request.TrademarkId, new Registration(request.CountryIso, request.RenewalPrice));
            var result = await _unitOfWork.CommitAsync();

            return result == 1;
        }
    }
}
