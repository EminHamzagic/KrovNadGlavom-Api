using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Data.DTO.Building;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace krov_nad_glavom_api.Application.Queries.Buildings
{
    public class GetAllBuildingsQueryHandler : IRequestHandler<GetAllBuildingsQuery, PaginatedResponse<BuildingToReturnDto>>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;
        public GetAllBuildingsQueryHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitofWork = unitofWork;
        }

        public async Task<PaginatedResponse<BuildingToReturnDto>> Handle(GetAllBuildingsQuery request, CancellationToken cancellationToken)
        {
            var (buildingsPage, totalCount, totalPages) = await _unitofWork.Buildings.GetAllValidBuildings(request.agencyId, request.parameters);

            var buildingsToRetrun = _mapper.Map<List<BuildingToReturnDto>>(buildingsPage);

            var companyIds = buildingsToRetrun.Select(b => b.CompanyId).ToList();
            var buildingIds = buildingsToRetrun.Select(b => b.Id).ToList();

            var companies = await _unitofWork.ConstructionCompanies.GetCompaniesByIds(companyIds);

            var companyDict = companies.ToDictionary(c => c.Id);


            foreach (var item in buildingsToRetrun)
            {
                if (companyDict.TryGetValue(item.CompanyId, out var company))
                    item.Company = company;
            }

            return new PaginatedResponse<BuildingToReturnDto>(
                buildingsToRetrun,
                totalCount,
                request.parameters.PageNumber,
                request.parameters.PageSize,
                totalPages
            );
        }
    }
}