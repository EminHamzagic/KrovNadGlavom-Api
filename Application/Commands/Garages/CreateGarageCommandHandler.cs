using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Garages
{
    public class CreateGarageCommandHandler : IRequestHandler<CreateGarageCommand, string>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public CreateGarageCommandHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<string> Handle(CreateGarageCommand request, CancellationToken cancellationToken)
        {
            var building = await _unitofWork.Buildings.GetBuildingById(request.GarageToAddDto.BuildingId);
            if ((_unitofWork.Garages.GetBuildingGarageCount(request.GarageToAddDto.BuildingId) + 1) > building.GarageSpotCount)
                throw new Exception("Njie moguće dodati novo garažno mesto, sva su popunjena");

            var isSpotFree = await _unitofWork.Garages.IsSpotNumberFree(request.GarageToAddDto.SpotNumber, building.Id);
            if (!isSpotFree)
                throw new Exception("Broj garažnog mesta je zauzet");

            var garage = _mapper.Map<Garage>(request.GarageToAddDto);
            garage.Id = Guid.NewGuid().ToString();
            _unitofWork.Garages.AddAsync(garage);
            await _unitofWork.Save();

            return garage.Id;
        }
    }
}