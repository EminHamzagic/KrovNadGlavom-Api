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

            _unitofWork.Apartments.Remove(apartment);
            await _unitofWork.Save();

            return apartment.Id;
        }
    }
}