using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.Agency;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Agencies
{
    public class GetAllAgenciesQueryHandler : IRequestHandler<GetAllAgenciesQuery, List<AgencyToReturnDto>>
    {
        private readonly IUnitofWork _unitofWork;
		private readonly IMapper _mapper;

		public GetAllAgenciesQueryHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
			_mapper = mapper;
		}

        public async Task<List<AgencyToReturnDto>> Handle(GetAllAgenciesQuery request, CancellationToken cancellationToken)
        {
            var agencies = await _unitofWork.Agencies.GetAllAsync();
            var agenciesToReturn = _mapper.Map<List<AgencyToReturnDto>>(agencies);

            foreach (var agency in agenciesToReturn)
            {
                agency.NumberOfBuildings = _unitofWork.AgencyRequests.GetAgencyBuildingCount(agency.Id);
                agency.NumberOfApartments = await _unitofWork.AgencyRequests.GetAgencyApartmentCount(agency.Id);
            }

            return agenciesToReturn;
        }
    }
}