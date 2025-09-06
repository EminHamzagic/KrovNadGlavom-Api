using krov_nad_glavom_api.Data.DTO.Agency;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Agencies
{
    public class UpdateAgencyCommand : IRequest<Agency>
    {
		public string Id { get; }
		public AgencyToAddDto AgencyToUpdateDto { get; }
        public UpdateAgencyCommand(string id, AgencyToAddDto agencyToUpdateDto)
        {
			Id = id;
			AgencyToUpdateDto = agencyToUpdateDto;
		}
	}
}