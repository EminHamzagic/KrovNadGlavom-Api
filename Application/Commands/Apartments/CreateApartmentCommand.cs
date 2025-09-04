using krov_nad_glavom_api.Data.DTO.Apartment;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Apartments
{
    public class CreateApartmentCommand : IRequest<string>
    {
		public ApartmentToAddDto ApartmentToAddDto { get; }
        public CreateApartmentCommand(ApartmentToAddDto apartmentToAddDto)
        {
			ApartmentToAddDto = apartmentToAddDto;
		}
	}
}