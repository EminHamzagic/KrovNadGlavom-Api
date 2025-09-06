using krov_nad_glavom_api.Data.DTO.Garage;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Garages
{
    public class CreateGarageCommand : IRequest<string>
    {
		public GarageToAddDto GarageToAddDto { get; }
        public CreateGarageCommand(GarageToAddDto garageToAddDto)
        {
			GarageToAddDto = garageToAddDto;
		}
	}
}