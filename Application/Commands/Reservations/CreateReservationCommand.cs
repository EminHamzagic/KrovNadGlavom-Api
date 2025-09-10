using krov_nad_glavom_api.Data.DTO.Reservation;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Reservations
{
    public class CreateReservationCommand : IRequest<Reservation>
    {
		public ReservationToAddDto ReservationToAddDto { get; }
        public CreateReservationCommand(ReservationToAddDto reservationToAddDto)
        {
			ReservationToAddDto = reservationToAddDto;
		}
	}
}