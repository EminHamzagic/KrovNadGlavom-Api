using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Buildings
{
    public class DeleteBuildingCommand : IRequest<Building>
    {
		public string Id { get; }
        public DeleteBuildingCommand(string id)
        {
			Id = id;
		}
	}
}