using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.AgencyRequests
{
    public record GetAgencyRequestsByAgencyIdQuery(string agencyId) : IRequest<List<AgencyRequest>>;
}