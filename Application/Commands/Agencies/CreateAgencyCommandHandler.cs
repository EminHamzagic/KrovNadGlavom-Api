using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Agencies
{
    public class CreateAgencyCommandHandler : IRequestHandler<CreateAgencyCommand, string>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public CreateAgencyCommandHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<string> Handle(CreateAgencyCommand request, CancellationToken cancellationToken)
        {
            var existingAgency = await _unitofWork.Agencies.GetAgencyByName(request.AgencyToAddDto.Name);
            if (existingAgency != null)
                throw new Exception("Agencija sa traženim imenom već postoji");

            var agency = _mapper.Map<Agency>(request.AgencyToAddDto);
            agency.Id = Guid.NewGuid().ToString();
            _unitofWork.Agencies.AddAsync(agency);
            await _unitofWork.Save();

            return agency.Id;
        }
    }
}