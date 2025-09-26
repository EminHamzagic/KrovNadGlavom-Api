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
            var buildingsQuery = _unitofWork.Buildings.GetBuildingsByCompanyId(request.comapnyId);
            buildingsQuery = buildingsQuery.Filter(request.parameters).Sort(request.parameters);

            var totalCount = buildingsQuery.Count();
            request.parameters.checkOverflow(totalCount);

            var buildingsPage = await buildingsQuery
                .Skip((request.parameters.PageNumber - 1) * request.parameters.PageSize)
                .Take(request.parameters.PageSize)
                .ToListAsync(cancellationToken);

             var totalPages = (int)Math.Ceiling((double)totalCount / request.parameters.PageSize);

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