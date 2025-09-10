using krov_nad_glavom_api.Application.Interfaces;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Reservations
{
    public class DeleteReservationCommandHandler : IRequestHandler<DeleteReservationCommand, bool>
    {
        private readonly IUnitofWork _unitofWork;

        public DeleteReservationCommandHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<bool> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _unitofWork.Reservations.GetByIdAsync(request.Id);
            if (reservation == null)
                throw new Exception("Rezervacija nije pronađena");

            _unitofWork.Reservations.Remove(reservation);
            await _unitofWork.Save();

            return true;
        }
    }
}