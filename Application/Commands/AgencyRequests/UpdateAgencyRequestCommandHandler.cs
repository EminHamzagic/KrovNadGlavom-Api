using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.AgencyRequests
{
    public class UpdateAgencyRequestCommandHandler : IRequestHandler<UpdateAgencyRequestCommand, AgencyRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateAgencyRequestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AgencyRequest> Handle(UpdateAgencyRequestCommand request, CancellationToken cancellationToken)
        {
            var agencyRequest = await _unitOfWork.AgencyRequests.GetByIdAsync(request.Id);
            if (agencyRequest == null)
                throw new Exception("Zahtev nije pronađen");

            _mapper.Map(request.AgencyRequestToUpdateDto, agencyRequest);
            _unitOfWork.AgencyRequests.Update(agencyRequest);

            if (request.AgencyRequestToUpdateDto.Status == "Approved")
            {
                var users = await _unitOfWork.UserAgencyFollows.GetAgencyFollowers(agencyRequest.AgencyId);
                if (users != null)
                {
                    var notifications = new List<Notification>();
                    foreach (var user in users)
                    {
                        notifications.Add(new Notification
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserId = user.Id,
                            Label = NotificationsLabelEnum.Novo,
                            Title = "Novi stanovi u ponudi",
                            Message = "Agencija koju pratite je uzela novu zgradu! Idite na stranicu Stanovi da bi ste pregledali nove stanove u ponudi.",
                            CreatedAt = DateTime.Now
                        });
                    }

                    await _unitOfWork.Notifications.AddRangeAsync(notifications);
                }
            }
            else if (request.AgencyRequestToUpdateDto.Status == "Rejected")
            {
                
            }

            await _unitOfWork.Save();

            return agencyRequest;
        }
    }
}