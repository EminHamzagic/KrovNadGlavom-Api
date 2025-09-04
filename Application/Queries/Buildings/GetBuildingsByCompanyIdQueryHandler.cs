using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Buildings
{
    public class GetBuildingsByCompanyIdQueryHandler : IRequestHandler<GetBuildingsByCompanyIdQuery, List<Building>>
    {
        private readonly IUnitofWork _unitofWork;

        public GetBuildingsByCompanyIdQueryHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<List<Building>> Handle(GetBuildingsByCompanyIdQuery request, CancellationToken cancellationToken)
        {
            var buildings = await _unitofWork.Buildings.GetBuildingsByCompanyId(request.comapnyId);
            return buildings;
        }   
    }
}