using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Buildings
{
    public class UpdateBuildingCommandHandler : IRequestHandler<UpdateBuildingCommand, Building>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public UpdateBuildingCommandHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<Building> Handle(UpdateBuildingCommand request, CancellationToken cancellationToken)
        {
            var building = await _unitofWork.Buildings.GetBuildingById(request.Id);
            if (building == null)
                throw new Exception("Zgrada nije pronađena");

            _mapper.Map(request.BuildingToUpdateDto, building);
            await _unitofWork.Save();

            return building;
        }
    }
}