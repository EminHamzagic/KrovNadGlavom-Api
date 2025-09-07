using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.AgencyRequest;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.AgencyRequests
{
    public class GetAgencyRequestsByCompanyIdQueryHandler : IRequestHandler<GetAgencyRequestsByCompanyIdQuery, List<AgencyRequestToReturnDto>>
    {
        private readonly IUnitofWork _unitofWork;
		private readonly IMapper _mapper;

		public GetAgencyRequestsByCompanyIdQueryHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
			_mapper = mapper;
		}

        public async Task<List<AgencyRequestToReturnDto>> Handle(GetAgencyRequestsByCompanyIdQuery request, CancellationToken cancellationToken)
        {
            var requests = await _unitofWork.AgencyRequests.GetAgencyRequestsByCompanyId(request.companyId);
            var company = await _unitofWork.ConstructionCompanies.GetByIdAsync(request.companyId);
            var requestsToReturn = _mapper.Map<List<AgencyRequestToReturnDto>>(requests);

            foreach (var item in requestsToReturn)
            {
                item.Agency = await _unitofWork.Agencies.GetByIdAsync(item.AgencyId);
                item.Building = await _unitofWork.Buildings.GetByIdAsync(item.BuildingId);
                item.Company = company;
            }
            return requestsToReturn;
        }
    }
}