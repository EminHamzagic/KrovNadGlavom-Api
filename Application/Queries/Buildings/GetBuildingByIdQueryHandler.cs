using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.Building;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Buildings
{
    public class GetBuildingByIdQueryHandler : IRequestHandler<GetBuildingByIdQuery, BuildingToReturnDto>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public GetBuildingByIdQueryHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<BuildingToReturnDto> Handle(GetBuildingByIdQuery request, CancellationToken cancellationToken)
        {
            var building = await _unitofWork.Buildings.GetBuildingById(request.id);
            if (building == null)
                throw new Exception("Zgrada nije pronađena");

            var apartments = await _unitofWork.Apartments.GetApartmentsByBuildingId(building.Id);
            var garages = await _unitofWork.Garages.GetGaragesByBuildingId(building.Id);
            var priceList = await _unitofWork.PriceLists.GetPriceListByBuildingId(building.Id);
            var company = await _unitofWork.ConstructionCompanies.GetByIdAsync(building.CompanyId);

            var buildingToReturn = _mapper.Map<BuildingToReturnDto>(building);
            buildingToReturn.Apartments = apartments;
            buildingToReturn.Garages = garages;
            buildingToReturn.PriceList = priceList;
            buildingToReturn.Company = company;
            buildingToReturn.RequestStatus = await _unitofWork.AgencyRequests.GetBuildingRequestStatus(building.Id);

            return buildingToReturn;
        }
    }
}