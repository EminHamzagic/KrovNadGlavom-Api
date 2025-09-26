using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Reservations
{
    public class GetReservationsQueryHandler : IRequestHandler<GetReservationsQuery, Reservation>
    {
        private readonly IUnitofWork _unitofWork;
        public GetReservationsQueryHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<Reservation> Handle(GetReservationsQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitofWork.Users.GetByIdAsync(request.userId);
            if (user == null)
                throw new Exception("Korisink nije pronađen");

            return await _unitofWork.Reservations.GetReservationByUserId(request.userId);
        }
    }
}