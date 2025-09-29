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
        private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetAllAgenciesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
			_mapper = mapper;
		}

        public async Task<PaginatedResponse<AgencyToReturnDto>> Handle(GetAllAgenciesQuery request, CancellationToken cancellationToken)
        {
            var (agenciesPage, totalCount, totalPages) = await _unitOfWork.Agencies.GetAgenciesQuery(request.parameters);

            var agenciesToReturn = _mapper.Map<List<AgencyToReturnDto>>(agenciesPage);

            foreach (var agency in agenciesToReturn)
            {
                agency.NumberOfBuildings = await _unitOfWork.AgencyRequests.GetAgencyBuildingCount(agency.Id);
                agency.NumberOfApartments = await _unitOfWork.AgencyRequests.GetAgencyApartmentCount(agency.Id);
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