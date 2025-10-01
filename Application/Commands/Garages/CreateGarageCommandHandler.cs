using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Garages
{
    public class CreateGarageCommandHandler : IRequestHandler<CreateGarageCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateGarageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<string> Handle(CreateGarageCommand request, CancellationToken cancellationToken)
        {
            var building = await _unitOfWork.Buildings.GetBuildingById(request.GarageToAddDto.BuildingId);
            if ((await _unitOfWork.Garages.GetBuildingGarageCount(request.GarageToAddDto.BuildingId) + 1) > building.GarageSpotCount)
                throw new Exception("Njie moguće dodati novo garažno mesto, sva su popunjena");

            var garage = _mapper.Map<Garage>(request.GarageToAddDto);

            var isSpotFree = await _unitOfWork.Garages.IsSpotNumberFree(request.GarageToAddDto.SpotNumber, garage);
            if (!isSpotFree)
                throw new Exception("Broj garažnog mesta je zauzet");

            garage.Id = Guid.NewGuid().ToString();
            await _unitOfWork.Garages.AddAsync(garage);
            await _unitOfWork.Save();

            return garage.Id;
        }
    }
}