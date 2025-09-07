using krov_nad_glavom_api.Data.DTO.AgencyRequest;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.AgencyRequests
{
    public class UpdateAgencyRequestCommand : IRequest<AgencyRequest>
    {
		public string Id { get; }
		public AgencyRequestToUpdateDto AgencyRequestToUpdateDto { get; }
        public UpdateAgencyRequestCommand(string id, AgencyRequestToUpdateDto agencyRequestToUpdateDto)
        {
			Id = id;
			AgencyRequestToUpdateDto = agencyRequestToUpdateDto;
		}
	}
}