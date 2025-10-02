using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.AgencyRequests
{
    public class CreateAgencyRequestCommandHandler : IRequestHandler<CreateAgencyRequestCommand, AgencyRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        public CreateAgencyRequestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService)
        {
            _notificationService = notificationService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<AgencyRequest> Handle(CreateAgencyRequestCommand request, CancellationToken cancellationToken)
        {
            var exisiting = await _unitOfWork.AgencyRequests.CheckForExistingRequest(request.AgencyRequestToAddDto);
            if (exisiting)
                throw new Exception("Već ste poslali zahtev za ovu zgradu");

            var agencyRequest = _mapper.Map<AgencyRequest>(request.AgencyRequestToAddDto);
            agencyRequest.Id = Guid.NewGuid().ToString();

            await _notificationService.SendNotificationsForAgencyRequestCreate(agencyRequest);

            await _unitOfWork.AgencyRequests.AddAsync(agencyRequest);
            await _unitOfWork.Save();

            return agencyRequest;
        }
    }
}