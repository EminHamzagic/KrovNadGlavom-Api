using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Agencies
{
    public class UpdateAgencyCommandHandler : IRequestHandler<UpdateAgencyCommand, Agency>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public UpdateAgencyCommandHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<Agency> Handle(UpdateAgencyCommand request, CancellationToken cancellationToken)
        {
            var agency = await _unitofWork.Agencies.GetByIdAsync(request.Id);
            if (agency == null)
                throw new Exception("Agencija nije pronađena");

            _mapper.Map(request.AgencyToUpdateDto, agency);
            _unitofWork.Agencies.Update(agency);
            await _unitofWork.Save();

            return agency;
        }
    }
}