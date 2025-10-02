using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Buildings
{
    public class ExtendBuildingEndDateCommandHandler : IRequestHandler<ExtendBuildingEndDateCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
		private readonly INotificationService _notificationService;

		public ExtendBuildingEndDateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
			_notificationService = notificationService;
		}

        public async Task<bool> Handle(ExtendBuildingEndDateCommand request, CancellationToken cancellationToken)
        {
            var building = await _unitOfWork.Buildings.GetByIdAsync(request.Id);
            if (building == null)
                throw new Exception("Zgrada nije pronađena");

            if (request.BuildingEndDateToExtendDto.ExtendedUntil < building.EndDate)
                throw new Exception("Datum produžetka ne može biti manji od prvobitnog datuma završetka zgrade");

            _mapper.Map(request.BuildingEndDateToExtendDto, building);

            await _notificationService.SendNotificationsForBuildingEndExtend(building);

            _unitOfWork.Buildings.Update(building);
            await _unitOfWork.Save();

            return true;
        }
    }
}