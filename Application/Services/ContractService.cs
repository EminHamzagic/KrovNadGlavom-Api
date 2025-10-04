using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using krov_nad_glavom_api.Domain.Entities;

namespace krov_nad_glavom_api.Application.Services
{
    public class ContractService : IContractService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;

        public ContractService(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _notificationService = notificationService;
            _unitOfWork = unitOfWork;
        }

        public async Task CheckUserContracts(User user)
        {
            var contracts = await _unitOfWork.Contracts.GetLatePaymentContracts(user);
            if (contracts != null)
            {
                foreach (var item in contracts)
                {
                    item.LateCount += 1;
                    if (item.LateCount == 4)
                    {
                        item.Status = "Invalid";
                        var apartment = await _unitOfWork.Apartments.GetByIdAsync(item.ApartmentId);
                        apartment.IsAvailable = true;
                        await _notificationService.SendNotificationsForContractInvalidated(item);
                        _unitOfWork.Apartments.Update(apartment);
                    }
                    else
                    {
                        await _notificationService.SendNotificationsForLateContractInstllment(item);
                    }

                    _unitOfWork.Contracts.Update(item);
                    await _unitOfWork.Save();
                }
            }
        }
    }
}