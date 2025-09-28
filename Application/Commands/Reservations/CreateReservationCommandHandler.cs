using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Reservations
{
    public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, Reservation>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public CreateReservationCommandHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<Reservation> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            var existingReservation = await _unitofWork.Reservations.GetReservationByUserId(request.ReservationToAddDto.UserId);
            if (existingReservation != null)
            {
                if (existingReservation.ToDate > DateTime.Now)
                    throw new Exception("Trenutno nije moguće napraviti novu rezervaciju jer imate aktivnu rezervaciju");
                else
                {
                    _unitofWork.Reservations.Remove(existingReservation);
                    await _unitofWork.Save();
                }
            }

            var reservation = _mapper.Map<Reservation>(request.ReservationToAddDto);
            reservation.Id = Guid.NewGuid().ToString();
            reservation.FromDate = DateTime.Now;
            reservation.ToDate = DateTime.Now.AddDays(5);

            await _unitofWork.Reservations.AddAsync(reservation);
            await _unitofWork.Save();

            return reservation;
        }
    }
}