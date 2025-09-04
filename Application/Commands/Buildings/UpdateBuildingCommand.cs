using krov_nad_glavom_api.Data.DTO.Building;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Buildings
{
    public class UpdateBuildingCommand : IRequest<Building>
    {
		public string Id { get; }
		public BuildingToUpdateDto BuildingToUpdateDto { get; }
        public UpdateBuildingCommand(string id, BuildingToUpdateDto buildingToUpdateDto)
        {
			Id = id;
			BuildingToUpdateDto = buildingToUpdateDto;
		}
        
	}
}