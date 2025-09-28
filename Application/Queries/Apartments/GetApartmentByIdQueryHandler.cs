using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.Apartment;
using krov_nad_glavom_api.Data.DTO.Building;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Apartments
{
    public class GetApartmentByIdQueryHandler : IRequestHandler<GetApartmentByIdQuery, ApartmentToReturnDto>
    {
        private readonly IUnitofWork _unitofWork;
		private readonly IMapper _mapper;

		public GetApartmentByIdQueryHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
			_mapper = mapper;
		}

        public async Task<ApartmentToReturnDto> Handle(GetApartmentByIdQuery request, CancellationToken cancellationToken)
        {
            var apartment = await _unitofWork.Apartments.GetApartmentById(request.id);
            if (apartment == null)
                throw new Exception("Stan nije pronađen");


            var apartmentToReturn = _mapper.Map<ApartmentToReturnDto>(apartment);
            var building = await _unitofWork.Buildings.GetByIdAsync(apartmentToReturn.BuildingId);
            var priceList = await _unitofWork.PriceLists.GetPriceListByBuildingId(building.Id);

            apartmentToReturn.Building = _mapper.Map<BuildingToReturnDto>(building);
            apartmentToReturn.Building.PriceList = priceList;
            apartmentToReturn.Agency = await _unitofWork.AgencyRequests.GetAgencyByBuildingId(apartmentToReturn.BuildingId);
            apartmentToReturn.Reservation = await _unitofWork.Reservations.GetByApartmentId(apartmentToReturn.Id);
            apartmentToReturn.CanRequestDiscount = await _unitofWork.Reservations.CanUserReserve(request.userId, apartmentToReturn.Agency.Id);
            apartmentToReturn.DiscountRequest = await _unitofWork.DiscountRequests.GetByApartmentAndUserId(apartmentToReturn.Id, request.userId);

            return apartmentToReturn;
        }
    }
}