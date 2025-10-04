using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.Apartment;
using krov_nad_glavom_api.Data.DTO.Building;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Apartments
{
    public class GetApartmentByIdQueryHandler : IRequestHandler<GetApartmentByIdQuery, ApartmentToReturnDto>
    {
        private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetApartmentByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
			_mapper = mapper;
		}

        public async Task<ApartmentToReturnDto> Handle(GetApartmentByIdQuery request, CancellationToken cancellationToken)
        {
            var apartment = await _unitOfWork.Apartments.GetApartmentById(request.id);
            if (apartment == null)
                throw new Exception("Stan nije pronađen");


            var apartmentToReturn = _mapper.Map<ApartmentToReturnDto>(apartment);
            var building = await _unitOfWork.Buildings.GetByIdAsync(apartmentToReturn.BuildingId);
            var priceList = await _unitOfWork.PriceLists.GetPriceListByBuildingId(building.Id);

            var exisitingContract = await _unitOfWork.Contracts.GetContractByApartmentIdAndUserId(request.id, request.userId);

            apartmentToReturn.IsAvailable = exisitingContract == null;
            apartmentToReturn.Building = _mapper.Map<BuildingToReturnDto>(building);
            apartmentToReturn.Building.PriceList = priceList;
            apartmentToReturn.Agency = await _unitOfWork.AgencyRequests.GetAgencyByBuildingId(apartmentToReturn.BuildingId);
            apartmentToReturn.Reservation = await _unitOfWork.Reservations.GetByApartmentId(apartmentToReturn.Id);
            apartmentToReturn.CanRequestDiscount = await _unitOfWork.Reservations.CanUserReserve(request.userId, apartmentToReturn.Agency.Id);
            apartmentToReturn.DiscountRequest = await _unitOfWork.DiscountRequests.GetByApartmentAndUserId(apartmentToReturn.Id, request.userId);
            apartmentToReturn.Garages = await _unitOfWork.Garages.GetGaragesByApartmentId(apartmentToReturn.Id);

            return apartmentToReturn;
        }
    }
}