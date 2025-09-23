using krov_nad_glavom_api.Data.DTO.Building;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Buildings
{
    public class ExtendBuildingEndDateCommand : IRequest<bool>
    {
		public string Id { get; }
		public BuildingEndDateToExtendDto BuildingEndDateToExtendDto { get; }
        public ExtendBuildingEndDateCommand(string id, BuildingEndDateToExtendDto buildingEndDateToExtendDto)
        {
			Id = id;
			BuildingEndDateToExtendDto = buildingEndDateToExtendDto;
		}
	}
}