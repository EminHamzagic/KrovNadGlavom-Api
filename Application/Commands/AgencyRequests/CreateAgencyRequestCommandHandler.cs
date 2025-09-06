using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.AgencyRequests
{
    public class CreateAgencyRequestCommandHandler : IRequestHandler<CreateAgencyRequestCommand, AgencyRequest>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;
        public CreateAgencyRequestCommandHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitofWork = unitofWork;
        }

        public async Task<AgencyRequest> Handle(CreateAgencyRequestCommand request, CancellationToken cancellationToken)
        {
            var exisiting = await _unitofWork.AgencyRequests.CheckForExistingRequest(request.AgencyRequestToAddDto);
            if (exisiting)
                throw new Exception("Već ste poslali zahtev za ovu zgradu");

            var agencyRequest = _mapper.Map<AgencyRequest>(request.AgencyRequestToAddDto);
            agencyRequest.Id = Guid.NewGuid().ToString();
            _unitofWork.AgencyRequests.AddAsync(agencyRequest);
            await _unitofWork.Save();

            return agencyRequest;
        }
    }
}