using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Agencies
{
    public class CreateAgencyCommandHandler : IRequestHandler<CreateAgencyCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateAgencyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<string> Handle(CreateAgencyCommand request, CancellationToken cancellationToken)
        {
            var existingAgency = await _unitOfWork.Agencies.GetAgencyByName(request.AgencyToAddDto.Name);
            if (existingAgency != null)
                throw new Exception("Agencija sa traženim imenom već postoji");

            var agency = _mapper.Map<Agency>(request.AgencyToAddDto);
            agency.Id = Guid.NewGuid().ToString();
            agency.IsAllowed = false;
            await _unitOfWork.Agencies.AddAsync(agency);
            await _unitOfWork.Save();

            return agency.Id;
        }
    }
}