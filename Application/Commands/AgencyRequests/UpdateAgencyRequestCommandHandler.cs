using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.AgencyRequests
{
    public class UpdateAgencyRequestCommandHandler : IRequestHandler<UpdateAgencyRequestCommand, AgencyRequest>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public UpdateAgencyRequestCommandHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<AgencyRequest> Handle(UpdateAgencyRequestCommand request, CancellationToken cancellationToken)
        {
            var agencyRequest = await _unitofWork.AgencyRequests.GetByIdAsync(request.Id);
            if (agencyRequest == null)
                throw new Exception("Zahtev nije pronađen");

            _mapper.Map(request.AgencyRequestToUpdateDto, agencyRequest);
            _unitofWork.AgencyRequests.Update(agencyRequest);
            await _unitofWork.Save();

            return agencyRequest;
        }
    }
}