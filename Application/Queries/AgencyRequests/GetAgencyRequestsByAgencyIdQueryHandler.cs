using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.AgencyRequest;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.AgencyRequests
{
    public class GetAgencyRequestsByAgencyIdQueryHandler : IRequestHandler<GetAgencyRequestsByAgencyIdQuery, List<AgencyRequestToReturnDto>>
    {
        private readonly IUnitofWork _unitofWork;
		private readonly IMapper _mapper;

		public GetAgencyRequestsByAgencyIdQueryHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
			_mapper = mapper;
		}

        public async Task<List<AgencyRequestToReturnDto>> Handle(GetAgencyRequestsByAgencyIdQuery request, CancellationToken cancellationToken)
        {
            var requests = await _unitofWork.AgencyRequests.GetAgencyRequestsByAgencyId(request.agencyId, request.status);
            var agency = await _unitofWork.Agencies.GetByIdAsync(request.agencyId);
            var requestsToReturn = _mapper.Map<List<AgencyRequestToReturnDto>>(requests.OrderByDescending(r => r.CreatedAt));

            foreach (var item in requestsToReturn)
            {
                item.Agency = agency;
                item.Building = await _unitofWork.Buildings.GetByIdAsync(item.BuildingId);
                item.Company = await _unitofWork.ConstructionCompanies.GetByIdAsync(item.Building.CompanyId);
            }
            return requestsToReturn;
        }
    }
}