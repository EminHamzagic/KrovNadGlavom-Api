using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.AgencyRequests
{
    public class CreateAgencyRequestCommandHandler : IRequestHandler<CreateAgencyRequestCommand, AgencyRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateAgencyRequestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
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
            var building = await _unitOfWork.Buildings.GetByIdAsync(agencyRequest.BuildingId);
            var comapnyUser = await _unitOfWork.Users.GetUserByCompanyId(building.CompanyId);

            var notification = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                UserId = comapnyUser.Id,
                Label = NotificationsLabelEnum.Zahtev,
                Title = "Novi zahtev za zgradu",
                Message = $@"Dobili ste novi zahtev za <a href='/buildings/{building.Id}' class='text-primary underline' target='_blank'>zgradu</a> od strance 
                            <a href='/agency/{agencyRequest.AgencyId}' class='text-primary underline' target='_blank'>agencije</a>. 
                            <span class='mt-5'>Idi na <a href='/requests' class='text-primary underline' target='_blank'>zahteve</a></span>",
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.Notifications.AddAsync(notification);
            await _unitOfWork.AgencyRequests.AddAsync(agencyRequest);
            await _unitOfWork.Save();

            return agencyRequest;
        }
    }
}