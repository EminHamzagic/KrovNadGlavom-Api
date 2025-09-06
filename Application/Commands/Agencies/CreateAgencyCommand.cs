using krov_nad_glavom_api.Data.DTO.Agency;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Agencies
{
    public class CreateAgencyCommand : IRequest<string>
    {
		public AgencyToAddDto AgencyToAddDto { get; }
        public CreateAgencyCommand(AgencyToAddDto agencyToAddDto)
        {
			AgencyToAddDto = agencyToAddDto;
		}
	}
}