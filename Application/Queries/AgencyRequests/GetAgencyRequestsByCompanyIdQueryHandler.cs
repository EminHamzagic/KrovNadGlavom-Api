using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.AgencyRequests
{
    public class GetAgencyRequestsByCompanyIdQueryHandler : IRequestHandler<GetAgencyRequestsByCompanyIdQuery, List<AgencyRequest>>
    {
        private readonly IUnitofWork _unitofWork;

        public GetAgencyRequestsByCompanyIdQueryHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<List<AgencyRequest>> Handle(GetAgencyRequestsByCompanyIdQuery request, CancellationToken cancellationToken)
        {
            var requests = await _unitofWork.AgencyRequests.GetAgencyRequestsByCompanyId(request.companyId);
            return requests;
        }
    }
}