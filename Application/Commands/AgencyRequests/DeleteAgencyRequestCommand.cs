using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.AgencyRequests
{
    public class DeleteAgencyRequestCommand : IRequest<AgencyRequest>
    {
		public string Id { get; }
        public DeleteAgencyRequestCommand(string id)
        {
			Id = id;
		}
	}
}