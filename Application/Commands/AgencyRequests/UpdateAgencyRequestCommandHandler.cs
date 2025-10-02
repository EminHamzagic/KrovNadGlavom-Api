using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.AgencyRequests
{
    public class UpdateAgencyRequestCommandHandler : IRequestHandler<UpdateAgencyRequestCommand, AgencyRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
		private readonly INotificationService _notificationService;

		public UpdateAgencyRequestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
			_notificationService = notificationService;
		}

        public async Task<AgencyRequest> Handle(UpdateAgencyRequestCommand request, CancellationToken cancellationToken)
        {
            var agencyRequest = await _unitOfWork.AgencyRequests.GetByIdAsync(request.Id);
            if (agencyRequest == null)
                throw new Exception("Zahtev nije pronađen");

            _mapper.Map(request.AgencyRequestToUpdateDto, agencyRequest);
            _unitOfWork.AgencyRequests.Update(agencyRequest);

            await _notificationService.SendNotificationsForAgencyRequestUpdate(request.AgencyRequestToUpdateDto, agencyRequest);

            await _unitOfWork.Save();

            return agencyRequest;
        }
    }
}