using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Buildings
{
    public class DeleteBuildingCommandHandler : IRequestHandler<DeleteBuildingCommand, Building>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        public DeleteBuildingCommandHandler(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _notificationService = notificationService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Building> Handle(DeleteBuildingCommand request, CancellationToken cancellationToken)
        {
            var building = await _unitOfWork.Buildings.GetByIdAsync(request.Id);
            if (building == null)
                throw new Exception("Zgrada nije pronađena");

            building.IsDeleted = true;
            var apartments = await _unitOfWork.Apartments.GetApartmentsByBuildingId(request.Id);
            foreach (var item in apartments)
            {
                item.IsDeleted = true;
                _unitOfWork.Apartments.Update(item);
            }

            var garages = await _unitOfWork.Garages.GetGaragesByBuildingId(request.Id);
            foreach (var item in garages)
            {
                item.IsDeleted = true;
                _unitOfWork.Garages.Update(item);
            }

            await _notificationService.SendNotificationsForBuildingDelete(building);

            _unitOfWork.Buildings.Update(building);
            await _unitOfWork.Save();

            return building;
        }
    }
}