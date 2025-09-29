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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllBuildingsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResponse<BuildingToReturnDto>> Handle(GetAllBuildingsQuery request, CancellationToken cancellationToken)
        {
            var (buildingsPage, totalCount, totalPages) = await _unitOfWork.Buildings.GetAllValidBuildings(request.agencyId, request.parameters);

            var buildingsToRetrun = _mapper.Map<List<BuildingToReturnDto>>(buildingsPage);

            var companyIds = buildingsToRetrun.Select(b => b.CompanyId).ToList();
            var buildingIds = buildingsToRetrun.Select(b => b.Id).ToList();

            var companies = await _unitOfWork.ConstructionCompanies.GetCompaniesByIds(companyIds);

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