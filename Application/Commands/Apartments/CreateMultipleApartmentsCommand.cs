using krov_nad_glavom_api.Data.DTO.Apartment;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Apartments
{
    public class CreateMultipleApartmentsCommand : IRequest<bool>
    {
		public MultipleApartmentsToAddDto MultipleApartmentsToAddDto { get; }
        public CreateMultipleApartmentsCommand(MultipleApartmentsToAddDto multipleApartmentsToAddDto)
        {
			MultipleApartmentsToAddDto = multipleApartmentsToAddDto;
		}
	}
}