using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Garages
{
    public class DeleteGarageCommandHandler : IRequestHandler<DeleteGarageCommand, Garage>
    {
        private readonly IUnitofWork _unitofWork;

        public DeleteGarageCommandHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<Garage> Handle(DeleteGarageCommand request, CancellationToken cancellationToken)
        {
            var garage = await _unitofWork.Garages.GetGarageById(request.Id);
            if (garage == null)
                throw new Exception("Garaža nije pronađena");

            _unitofWork.Garages.Remove(garage);
            await _unitofWork.Save();

            return garage;
        }
    }
}