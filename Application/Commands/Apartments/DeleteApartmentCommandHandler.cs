using krov_nad_glavom_api.Application.Interfaces;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Apartments
{
    public class DeleteApartmentCommandHandler : IRequestHandler<DeleteApartmentCommand, string>
    {
        private readonly IUnitofWork _unitofWork;

        public DeleteApartmentCommandHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<string> Handle(DeleteApartmentCommand request, CancellationToken cancellationToken)
        {
            var apartment = await _unitofWork.Apartments.GetApartmentById(request.Id);
            if (apartment == null)
                throw new Exception("Stan nije pronađen");

            var contract = await _unitofWork.Contracts.GetContractsByApartmentId(apartment.Id);
            var discountRequest = await _unitofWork.DiscountRequests.GetByApartmentId(apartment.Id);
            var reservation = await _unitofWork.Reservations.GetByApartmentId(apartment.Id);

            if (contract != null)
                throw new Exception("Nije moguće izbrisati stan jer se nalazi pod ugovorom");

            if (discountRequest != null)
            {
                _unitofWork.DiscountRequests.Remove(discountRequest);
                // Dodati novi unos u tabelu za notifikacije o tome da je stan za ovaj zahtev za popust izbrisan
            }

            if (reservation != null)
            {
                _unitofWork.Reservations.Remove(reservation);
                // Dodati novi unos u tabelu za notifikacije o tome da je stan za ovu rezervaciju izbrisan
            }

            _unitofWork.Apartments.Remove(apartment);
            await _unitofWork.Save();

            return apartment.Id;
        }
    }
}