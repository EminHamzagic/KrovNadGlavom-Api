using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.AgencyRequests
{
    public record GetAgencyRequestsByCompanyIdQuery(string companyId) : IRequest<List<AgencyRequest>>;
}