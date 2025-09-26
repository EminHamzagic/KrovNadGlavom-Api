using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Data.DTO.Agency;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Agencies
{
    public record GetAllAgenciesQuery(QueryStringParameters parameters) : IRequest<PaginatedResponse<AgencyToReturnDto>>;
}