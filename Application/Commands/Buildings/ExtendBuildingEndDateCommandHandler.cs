using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Buildings
{
    public class ExtendBuildingEndDateCommandHandler : IRequestHandler<ExtendBuildingEndDateCommand, bool>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public ExtendBuildingEndDateCommandHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<bool> Handle(ExtendBuildingEndDateCommand request, CancellationToken cancellationToken)
        {
            var building = await _unitofWork.Buildings.GetByIdAsync(request.Id);
            if (building == null)
                throw new Exception("Zgrada nije pronađena");

            if (request.BuildingEndDateToExtendDto.ExtendedUntil < building.EndDate)
                throw new Exception("Datum produžetka ne može biti manji od prvobitnog datuma završetka zgrade");

            _mapper.Map(request.BuildingEndDateToExtendDto, building);
            await _unitofWork.Save();

            return true;
        }
    }
}