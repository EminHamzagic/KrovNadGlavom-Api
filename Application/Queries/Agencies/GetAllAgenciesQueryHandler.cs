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
            var agenciesQuery = _unitofWork.Agencies.GetAgenciesQuery();
            agenciesQuery = agenciesQuery.Filter(request.parameters).Sort(request.parameters);

            var totalCount = agenciesQuery.Count();
            request.parameters.checkOverflow(totalCount);

            var agenciesPage = await agenciesQuery
                .Skip((request.parameters.PageNumber - 1) * request.parameters.PageSize)
                .Take(request.parameters.PageSize)
                .ToListAsync(cancellationToken);

            var agenciesToReturn = _mapper.Map<List<AgencyToReturnDto>>(agenciesPage);

            foreach (var agency in agenciesToReturn)
            {
                agency.NumberOfBuildings = _unitofWork.AgencyRequests.GetAgencyBuildingCount(agency.Id);
                agency.NumberOfApartments = await _unitofWork.AgencyRequests.GetAgencyApartmentCount(agency.Id);
            }

            var totalPages = (int)Math.Ceiling((double)totalCount / request.parameters.PageSize);

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