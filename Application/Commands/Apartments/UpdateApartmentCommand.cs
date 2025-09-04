using krov_nad_glavom_api.Data.DTO.Apartment;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Apartments
{
    public class UpdateApartmentCommand : IRequest<Apartment>
    {
		public ApartmentToUpdateDto ApartmentToUpdateDto { get; }
		public string Id { get; }

		public UpdateApartmentCommand(ApartmentToUpdateDto apartmentToUpdateDto, string id)
        {
            ApartmentToUpdateDto = apartmentToUpdateDto;
			Id = id;
		}
	}
}