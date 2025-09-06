using krov_nad_glavom_api.Data.DTO.AgencyRequest;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.AgencyRequests
{
    public class CreateAgencyRequestCommand : IRequest<AgencyRequest>
    {
		public AgencyRequestToAddDto AgencyRequestToAddDto { get; }
        public CreateAgencyRequestCommand(AgencyRequestToAddDto agencyRequestToAddDto)
        {
			AgencyRequestToAddDto = agencyRequestToAddDto;
		}
	}
}