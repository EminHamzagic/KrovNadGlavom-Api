using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Buildings
{
    public class GetBuildingsPageQueryHandler : IRequestHandler<GetBuildingsPageQuery, PaginatedResponse<Building>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetBuildingsPageQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResponse<Building>> Handle(GetBuildingsPageQuery request, CancellationToken cancellationToken)
        {
            var (buildingsPage, totalCount, totalPages) = await _unitOfWork.Buildings.GetBuildingsPage(request.parameters);

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