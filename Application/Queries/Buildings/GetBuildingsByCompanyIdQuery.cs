using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Buildings
{
    public record GetBuildingsByCompanyIdQuery(string comapnyId) : IRequest<List<Building>>;
}