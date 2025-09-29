using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Agencies
{
    public class UpdateAgencyCommandHandler : IRequestHandler<UpdateAgencyCommand, Agency>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateAgencyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Agency> Handle(UpdateAgencyCommand request, CancellationToken cancellationToken)
        {
            var agency = await _unitOfWork.Agencies.GetByIdAsync(request.Id);
            if (agency == null)
                throw new Exception("Agencija nije pronađena");

            _mapper.Map(request.AgencyToUpdateDto, agency);
            _unitOfWork.Agencies.Update(agency);
            await _unitOfWork.Save();

            return agency;
        }
    }
}