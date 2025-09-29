using krov_nad_glavom_api.Application.Interfaces;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Reservations
{
    public class DeleteReservationCommandHandler : IRequestHandler<DeleteReservationCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteReservationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _unitOfWork.Reservations.GetByIdAsync(request.Id);
            if (reservation == null)
                throw new Exception("Rezervacija nije pronađena");

            _unitOfWork.Reservations.Remove(reservation);
            await _unitOfWork.Save();

            return true;
        }
    }
}