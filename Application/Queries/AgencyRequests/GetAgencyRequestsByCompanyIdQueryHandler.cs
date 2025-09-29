using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.AgencyRequest;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.AgencyRequests
{
    public class GetAgencyRequestsByCompanyIdQueryHandler : IRequestHandler<GetAgencyRequestsByCompanyIdQuery, List<AgencyRequestToReturnDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetAgencyRequestsByCompanyIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
			_mapper = mapper;
		}

        public async Task<List<AgencyRequestToReturnDto>> Handle(GetAgencyRequestsByCompanyIdQuery request, CancellationToken cancellationToken)
        {
            var requests = await _unitOfWork.AgencyRequests.GetAgencyRequestsByCompanyId(request.companyId, request.status);
            var company = await _unitOfWork.ConstructionCompanies.GetByIdAsync(request.companyId);
            var requestsToReturn = _mapper.Map<List<AgencyRequestToReturnDto>>(requests.OrderByDescending(r => r.CreatedAt));

            foreach (var item in requestsToReturn)
            {
                item.Agency = await _unitOfWork.Agencies.GetByIdAsync(item.AgencyId);
                item.Building = await _unitOfWork.Buildings.GetByIdAsync(item.BuildingId);
                item.Company = company;
            }
            return requestsToReturn;
        }
    }
}