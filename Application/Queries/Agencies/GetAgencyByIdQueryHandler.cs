using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.Agency;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Agencies
{
    public class GetAgencyByIdQueryHandler : IRequestHandler<GetAgencyByIdQuery, AgencyToReturnDto>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public GetAgencyByIdQueryHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<AgencyToReturnDto> Handle(GetAgencyByIdQuery request, CancellationToken cancellationToken)
        {
            var agency = await _unitofWork.Agencies.GetByIdAsync(request.Id);
            if (agency == null)
                throw new Exception("Agencija nije pronađena");

            var agencyToReturn = _mapper.Map<AgencyToReturnDto>(agency);

            agencyToReturn.NumberOfBuildings = _unitofWork.AgencyRequests.GetAgencyBuildingCount(agency.Id);
            agencyToReturn.NumberOfApartments = await _unitofWork.AgencyRequests.GetAgencyApartmentCount(agency.Id);
            agencyToReturn.Follow = await _unitofWork.UserAgencyFollows.IsUserFollowing(request.userId, request.Id);

            return agencyToReturn;
        }
    }
}