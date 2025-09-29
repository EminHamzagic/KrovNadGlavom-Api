using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.Agency;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Agencies
{
    public class GetAgencyByIdQueryHandler : IRequestHandler<GetAgencyByIdQuery, AgencyToReturnDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAgencyByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AgencyToReturnDto> Handle(GetAgencyByIdQuery request, CancellationToken cancellationToken)
        {
            var agency = await _unitOfWork.Agencies.GetByIdAsync(request.Id);
            if (agency == null)
                throw new Exception("Agencija nije pronađena");

            var agencyToReturn = _mapper.Map<AgencyToReturnDto>(agency);

            agencyToReturn.NumberOfBuildings = await _unitOfWork.AgencyRequests.GetAgencyBuildingCount(agency.Id);
            agencyToReturn.NumberOfApartments = await _unitOfWork.AgencyRequests.GetAgencyApartmentCount(agency.Id);
            agencyToReturn.Follow = await _unitOfWork.UserAgencyFollows.IsUserFollowing(request.userId, request.Id);

            return agencyToReturn;
        }
    }
}