using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Garages
{
    public class DeleteGarageCommand : IRequest<Garage>
    {
		public string Id { get; }
        public DeleteGarageCommand(string id)
        {
			Id = id;
		}
	}
}