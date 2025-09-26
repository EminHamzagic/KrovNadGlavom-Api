using krov_nad_glavom_api.Application.Interfaces;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.DiscountRequests
{
    public class ForwardDiscountRequestCommandHandler : IRequestHandler<ForwardDiscountRequestCommand, string>
    {
        private readonly IUnitofWork _unitofWork;
        public ForwardDiscountRequestCommandHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<string> Handle(ForwardDiscountRequestCommand request, CancellationToken cancellationToken)
        {
            var discountRequest = await _unitofWork.DiscountRequests.GetByIdAsync(request.RequestId);
            if (discountRequest == null)
                throw new Exception("Zahtev za popust nije pronađen");

            var apartment = await _unitofWork.Apartments.GetByIdAsync(discountRequest.ApartmentId);
            var building = await _unitofWork.Buildings.GetByIdAsync(apartment.BuildingId);

            discountRequest.ConstructionCompanyId = building.CompanyId;
            _unitofWork.DiscountRequests.Update(discountRequest);
            await _unitofWork.Save();

            return discountRequest.Id;
        }
    }
}