using krov_nad_glavom_api.Data.DTO.Agency;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Agencies
{
    public record GetAgencyByIdQuery(string Id, string userId) : IRequest<AgencyToReturnDto>;
}