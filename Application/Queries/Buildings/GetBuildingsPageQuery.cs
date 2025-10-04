using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Buildings
{
    public record GetBuildingsPageQuery(QueryStringParameters parameters) : IRequest<PaginatedResponse<Building>>;
}