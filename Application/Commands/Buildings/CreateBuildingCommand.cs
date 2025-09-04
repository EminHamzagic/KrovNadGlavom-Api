using krov_nad_glavom_api.Data.DTO.Building;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Buildings
{
    public class CreateBuildingCommand : IRequest<string>
    {
		public BuildingToAddDto BuildingToAddDto { get; }
        public CreateBuildingCommand(BuildingToAddDto buildingToAddDto)
        {
			BuildingToAddDto = buildingToAddDto;
		}
	}
}