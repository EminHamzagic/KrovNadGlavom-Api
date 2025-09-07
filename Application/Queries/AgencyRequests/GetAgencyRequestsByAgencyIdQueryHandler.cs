using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.AgencyRequests
{
    public class GetAgencyRequestsByAgencyIdQueryHandler : IRequestHandler<GetAgencyRequestsByAgencyIdQuery, List<AgencyRequest>>
    {
        private readonly IUnitofWork _unitofWork;

        public GetAgencyRequestsByAgencyIdQueryHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<List<AgencyRequest>> Handle(GetAgencyRequestsByAgencyIdQuery request, CancellationToken cancellationToken)
        {
            var requests = await _unitofWork.AgencyRequests.GetAgencyRequestsByAgencyId(request.agencyId);
            return requests;
        }
    }
}