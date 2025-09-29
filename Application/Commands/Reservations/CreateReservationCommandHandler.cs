using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Reservations
{
    public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, Reservation>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateReservationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Reservation> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            var existingReservation = await _unitOfWork.Reservations.GetReservationByUserId(request.ReservationToAddDto.UserId);
            if (existingReservation != null)
            {
                if (existingReservation.ToDate > DateTime.Now)
                    throw new Exception("Trenutno nije moguće napraviti novu rezervaciju jer imate aktivnu rezervaciju");
                else
                {
                    _unitOfWork.Reservations.Remove(existingReservation);
                    await _unitOfWork.Save();
                }
            }

            var reservation = _mapper.Map<Reservation>(request.ReservationToAddDto);
            reservation.Id = Guid.NewGuid().ToString();
            reservation.FromDate = DateTime.Now;
            reservation.ToDate = DateTime.Now.AddDays(5);

            await _unitOfWork.Reservations.AddAsync(reservation);
            await _unitOfWork.Save();

            return reservation;
        }
    }
}