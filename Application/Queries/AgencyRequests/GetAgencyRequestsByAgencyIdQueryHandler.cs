using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.AgencyRequest;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.AgencyRequests
{
    public class GetAgencyRequestsByAgencyIdQueryHandler : IRequestHandler<GetAgencyRequestsByAgencyIdQuery, List<AgencyRequestToReturnDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetAgencyRequestsByAgencyIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
			_mapper = mapper;
		}

        public async Task<List<AgencyRequestToReturnDto>> Handle(GetAgencyRequestsByAgencyIdQuery request, CancellationToken cancellationToken)
        {
            var requests = await _unitOfWork.AgencyRequests.GetAgencyRequestsByAgencyId(request.agencyId, request.status);
            var agency = await _unitOfWork.Agencies.GetByIdAsync(request.agencyId);
            var requestsToReturn = _mapper.Map<List<AgencyRequestToReturnDto>>(requests.OrderByDescending(r => r.CreatedAt));

            foreach (var item in requestsToReturn)
            {
                item.Agency = agency;
                item.Building = await _unitOfWork.Buildings.GetByIdAsync(item.BuildingId);
                item.Company = await _unitOfWork.ConstructionCompanies.GetByIdAsync(item.Building.CompanyId);
            }
            return requestsToReturn;
        }
    }
}