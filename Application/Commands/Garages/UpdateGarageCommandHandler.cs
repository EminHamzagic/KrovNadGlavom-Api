using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Garages
{
    public class UpdateGarageCommandHandler : IRequestHandler<UpdateGarageCommand, Garage>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public UpdateGarageCommandHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<Garage> Handle(UpdateGarageCommand request, CancellationToken cancellationToken)
        {
            var garage = await _unitofWork.Garages.GetGarageById(request.Id);
            if (garage == null)
                throw new Exception("Garaža nije pronađena");

            bool isSpotFree = await _unitofWork.Garages.IsSpotNumberFree(request.GarageToUpdateDto.SpotNumber, garage.BuildingId);
            if (!isSpotFree)
                throw new Exception("Broj garažnog mesta je zauzet");

            _mapper.Map(request.GarageToUpdateDto, garage);
            _unitofWork.Garages.Update(garage);

            return garage;
        }
    }
}