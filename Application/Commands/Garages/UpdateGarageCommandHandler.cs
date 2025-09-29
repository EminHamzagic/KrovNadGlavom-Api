using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Garages
{
    public class UpdateGarageCommandHandler : IRequestHandler<UpdateGarageCommand, Garage>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateGarageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Garage> Handle(UpdateGarageCommand request, CancellationToken cancellationToken)
        {
            var garage = await _unitOfWork.Garages.GetGarageById(request.Id);
            if (garage == null)
                throw new Exception("Garaža nije pronađena");

            bool isSpotFree = await _unitOfWork.Garages.IsSpotNumberFree(request.GarageToUpdateDto.SpotNumber, garage.BuildingId);
            if (!isSpotFree)
                throw new Exception("Broj garažnog mesta je zauzet");

            _mapper.Map(request.GarageToUpdateDto, garage);
            _unitOfWork.Garages.Update(garage);

            return garage;
        }
    }
}