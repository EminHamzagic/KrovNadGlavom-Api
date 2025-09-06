using krov_nad_glavom_api.Data.DTO.Building;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Buildings
{
    public record GetBuildingByIdQuery(string id) : IRequest<BuildingToReturnDto>;
}