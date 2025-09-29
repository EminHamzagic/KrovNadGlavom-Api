using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Garages
{
    public class DeleteGarageCommandHandler : IRequestHandler<DeleteGarageCommand, Garage>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteGarageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Garage> Handle(DeleteGarageCommand request, CancellationToken cancellationToken)
        {
            var garage = await _unitOfWork.Garages.GetGarageById(request.Id);
            if (garage == null)
                throw new Exception("Garaža nije pronađena");

            _unitOfWork.Garages.Remove(garage);
            await _unitOfWork.Save();

            return garage;
        }
    }
}