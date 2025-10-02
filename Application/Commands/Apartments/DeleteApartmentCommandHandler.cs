using krov_nad_glavom_api.Application.Interfaces;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Apartments
{
    public class DeleteApartmentCommandHandler : IRequestHandler<DeleteApartmentCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteApartmentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(DeleteApartmentCommand request, CancellationToken cancellationToken)
        {
            var apartment = await _unitOfWork.Apartments.GetApartmentById(request.Id);
            if (apartment == null)
                throw new Exception("Stan nije pronađen");

            var contract = await _unitOfWork.Contracts.GetContractByApartmentId(apartment.Id);
            var discountRequest = await _unitOfWork.DiscountRequests.GetByApartmentId(apartment.Id);
            var reservation = await _unitOfWork.Reservations.GetByApartmentId(apartment.Id);

            if (contract != null)
                throw new Exception("Nije moguće izbrisati stan jer se nalazi pod ugovorom");

            if (discountRequest != null)
            {
                _unitOfWork.DiscountRequests.Remove(discountRequest);
                // Dodati novi unos u tabelu za notifikacije o tome da je stan za ovaj zahtev za popust izbrisan
            }

            if (reservation != null)
            {
                _unitOfWork.Reservations.Remove(reservation);
                // Dodati novi unos u tabelu za notifikacije o tome da je stan za ovu rezervaciju izbrisan
            }

            _unitOfWork.Apartments.Remove(apartment);
            await _unitOfWork.Save();

            return apartment.Id;
        }
    }
}