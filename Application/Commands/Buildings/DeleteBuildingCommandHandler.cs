using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Buildings
{
    public class DeleteBuildingCommandHandler : IRequestHandler<DeleteBuildingCommand, Building>
    {
        private readonly IUnitofWork _unitofWork;
        public DeleteBuildingCommandHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<Building> Handle(DeleteBuildingCommand request, CancellationToken cancellationToken)
        {
            var building = await _unitofWork.Buildings.GetByIdAsync(request.Id);
            if (building == null)
                throw new Exception("Zgrada nije pronađena");

            _unitofWork.Buildings.Remove(building);
            await _unitofWork.Save();

            return building;
        }
    }
}