using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.Building;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Buildings
{
    public class GetAllBuildingsQueryHandler : IRequestHandler<GetAllBuildingsQuery, List<BuildingToReturnDto>>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;
        public GetAllBuildingsQueryHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitofWork = unitofWork;
        }

        public async Task<List<BuildingToReturnDto>> Handle(GetAllBuildingsQuery request, CancellationToken cancellationToken)
        {
            var buildings = await _unitofWork.Buildings.GetAllValidBuildings(request.agencyId);

            var companyIds = buildings.Select(b => b.CompanyId).ToList();
            var buildingIds = buildings.Select(b => b.Id).ToList();

            var companies = await _unitofWork.ConstructionCompanies.GetCompaniesByIds(companyIds);

            var companyDict = companies.ToDictionary(c => c.Id);

            var buildingsToRetrun = _mapper.Map<List<BuildingToReturnDto>>(buildings);

            foreach (var item in buildingsToRetrun)
            {
                if (companyDict.TryGetValue(item.CompanyId, out var company))
                    item.Company = company;
            }

            return buildingsToRetrun;
        }
    }
}