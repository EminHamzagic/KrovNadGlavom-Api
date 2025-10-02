using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.DiscountRequests
{
    public class ForwardDiscountRequestCommandHandler : IRequestHandler<ForwardDiscountRequestCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        public ForwardDiscountRequestCommandHandler(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _notificationService = notificationService;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(ForwardDiscountRequestCommand request, CancellationToken cancellationToken)
        {
            var discountRequest = await _unitOfWork.DiscountRequests.GetByIdAsync(request.RequestId);
            if (discountRequest == null)
                throw new Exception("Zahtev za popust nije pronađen");

            var apartment = await _unitOfWork.Apartments.GetByIdAsync(discountRequest.ApartmentId);
            var building = await _unitOfWork.Buildings.GetByIdAsync(apartment.BuildingId);

            discountRequest.ConstructionCompanyId = building.CompanyId;
            _unitOfWork.DiscountRequests.Update(discountRequest);
            await _notificationService.SendNotificationsForDiscountRequestForward(discountRequest);
            await _unitOfWork.Save();

            return discountRequest.Id;
        }
    }
}