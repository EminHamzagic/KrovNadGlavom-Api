using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.Building;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Buildings
{
    public class GetBuildingByIdQueryHandler : IRequestHandler<GetBuildingByIdQuery, BuildingToReturnDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBuildingByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BuildingToReturnDto> Handle(GetBuildingByIdQuery request, CancellationToken cancellationToken)
        {
            var building = await _unitOfWork.Buildings.GetBuildingById(request.id);
            if (building == null)
                throw new Exception("Zgrada nije pronađena");

            var apartments = await _unitOfWork.Apartments.GetApartmentsByBuildingId(building.Id);
            var garages = await _unitOfWork.Garages.GetGaragesByBuildingId(building.Id);
            var priceList = await _unitOfWork.PriceLists.GetPriceListByBuildingId(building.Id);
            var company = await _unitOfWork.ConstructionCompanies.GetByIdAsync(building.CompanyId);

            var buildingToReturn = _mapper.Map<BuildingToReturnDto>(building);
            buildingToReturn.Apartments = apartments;
            buildingToReturn.Garages = garages;
            buildingToReturn.PriceList = priceList;
            buildingToReturn.Company = company;
            buildingToReturn.RequestStatus = await _unitOfWork.AgencyRequests.GetBuildingRequestStatus(building.Id);

            return buildingToReturn;
        }
    }
}