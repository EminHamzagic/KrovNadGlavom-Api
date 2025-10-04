using krov_nad_glavom_api.Application.Interfaces;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
            if (user == null)
                throw new Exception("Korisnik nije pronađen");

            user.IsDeleted = true;
            user.Email = $"{Guid.NewGuid()}_deleted_{user.Email}";
            _unitOfWork.Users.Update(user);

            var discountRequests = await _unitOfWork.DiscountRequests.GetByUserId(user.Id);
            if (discountRequests != null && discountRequests.Any())
            {
                _unitOfWork.DiscountRequests.RemoveRange(discountRequests);
            }

            var notifications = await _unitOfWork.Notifications.GetNotificationsByUserId(user.Id);
            if (notifications != null && notifications.Any())
            {
                _unitOfWork.Notifications.RemoveRange(notifications);
            }

            var reservation = await _unitOfWork.Reservations.GetReservationByUserId(user.Id);
            if (reservation != null)
            {
                _unitOfWork.Reservations.Remove(reservation);
            }

            var follows = await _unitOfWork.UserAgencyFollows.GetByUserId(user.Id);
            if (follows != null && follows.Any())
            {
                _unitOfWork.UserAgencyFollows.RemoveRange(follows);
            }

            var contracts = await _unitOfWork.Contracts.GetByUserId(user.Id);
            if (contracts != null && contracts.Any())
            {
                foreach (var item in contracts)
                {
                    var apartment = await _unitOfWork.Apartments.GetByIdAsync(item.ApartmentId);
                    apartment.IsAvailable = true;
                    _unitOfWork.Apartments.Update(apartment);
                }
                
                _unitOfWork.Contracts.RemoveRange(contracts);
            }


            await _unitOfWork.Save();

            return true;
        }
    }
}