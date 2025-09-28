using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Data.DTO.Agency;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace krov_nad_glavom_api.Application.Queries.Agencies
{
    public class GetAllAgenciesQueryHandler : IRequestHandler<GetAllAgenciesQuery, PaginatedResponse<AgencyToReturnDto>>
    {
        private readonly IUnitofWork _unitofWork;
		private readonly IMapper _mapper;

		public GetAllAgenciesQueryHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
			_mapper = mapper;
		}

        public async Task<PaginatedResponse<AgencyToReturnDto>> Handle(GetAllAgenciesQuery request, CancellationToken cancellationToken)
        {
            var (agenciesPage, totalCount, totalPages) = await _unitofWork.Agencies.GetAgenciesQuery(request.parameters);

            var agenciesToReturn = _mapper.Map<List<AgencyToReturnDto>>(agenciesPage);

            foreach (var agency in agenciesToReturn)
            {
                agency.NumberOfBuildings = await _unitofWork.AgencyRequests.GetAgencyBuildingCount(agency.Id);
                agency.NumberOfApartments = await _unitofWork.AgencyRequests.GetAgencyApartmentCount(agency.Id);
            }

            return new PaginatedResponse<AgencyToReturnDto>(
                agenciesToReturn,
                totalCount,
                request.parameters.PageNumber,
                request.parameters.PageSize,
                totalPages
            );
        }
    }
}