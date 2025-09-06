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

            building.IsDeleted = true;
            var apartments = await _unitofWork.Apartments.GetApartmentsByBuildingId(request.Id);
            foreach (var item in apartments)
            {
                item.IsDeleted = true;
            }

            var garages = await _unitofWork.Garages.GetGaragesByBuildingId(request.Id);
            foreach (var item in garages)
            {
                item.IsDeleted = true;
            }
            
            await _unitofWork.Save();

            return building;
        }
    }
}