using krov_nad_glavom_api.Data.DTO.Garage;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Garages
{
    public class UpdateGarageCommand : IRequest<Garage>
    {
		public string Id { get; }
		public GarageToUpdateDto GarageToUpdateDto { get; }
        public UpdateGarageCommand(string id, GarageToUpdateDto garageToUpdateDto)
        {
			Id = id;
			GarageToUpdateDto = garageToUpdateDto;
		}
	}
}