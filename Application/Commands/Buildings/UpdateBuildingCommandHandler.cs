using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Buildings
{
    public class UpdateBuildingCommandHandler : IRequestHandler<UpdateBuildingCommand, Building>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateBuildingCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Building> Handle(UpdateBuildingCommand request, CancellationToken cancellationToken)
        {
            var building = await _unitOfWork.Buildings.GetBuildingById(request.Id);
            if (building == null)
                throw new Exception("Zgrada nije pronađena");

            _mapper.Map(request.BuildingToUpdateDto, building);
            _unitOfWork.Buildings.Update(building);
            await _unitOfWork.Save();

            return building;
        }
    }
}