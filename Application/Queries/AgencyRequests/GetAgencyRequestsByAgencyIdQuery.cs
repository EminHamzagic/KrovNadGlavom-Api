using krov_nad_glavom_api.Data.DTO.AgencyRequest;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.AgencyRequests
{
    public record GetAgencyRequestsByAgencyIdQuery(string agencyId, string status) : IRequest<List<AgencyRequestToReturnDto>>;
}