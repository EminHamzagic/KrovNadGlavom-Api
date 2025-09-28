using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace krov_nad_glavom_api.Application.Queries.Buildings
{
    public class GetBuildingsByCompanyIdQueryHandler : IRequestHandler<GetBuildingsByCompanyIdQuery, PaginatedResponse<Building>>
    {
        private readonly IUnitofWork _unitofWork;

        public GetBuildingsByCompanyIdQueryHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<PaginatedResponse<Building>> Handle(GetBuildingsByCompanyIdQuery request, CancellationToken cancellationToken)
        {
            var (buildingsPage, totalCount, totalPages) = await _unitofWork.Buildings.GetCompanyBuildings(request.comapnyId, request.parameters);

            return new PaginatedResponse<Building>(
                buildingsPage,
                totalCount,
                request.parameters.PageNumber,
                request.parameters.PageSize,
                totalPages
            );
        }   
    }
}