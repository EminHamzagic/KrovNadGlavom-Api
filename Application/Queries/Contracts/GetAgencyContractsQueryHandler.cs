using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.Apartment;
using krov_nad_glavom_api.Data.DTO.Contract;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Contracts
{
    public class GetAgencyContractsQueryHandler : IRequestHandler<GetAgencyContractsQuery, List<ContractToReturnDto>>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public GetAgencyContractsQueryHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }
        
        public async Task<List<ContractToReturnDto>> Handle(GetAgencyContractsQuery request, CancellationToken cancellationToken)
        {
            var contracts = await _unitofWork.Contracts.GetContractsByAgencyId(request.agencyId);

            var userIds = contracts.Select(c => c.UserId).Distinct().ToList();
            var apartmentIds = contracts.Select(c => c.ApartmentId).Distinct().ToList();

            var users = await _unitofWork.Users.GetUsersByIds(userIds);
            var apartments = await _unitofWork.Apartments.GetApartmentsByIds(apartmentIds);
            var agency = await _unitofWork.Agencies.GetByIdAsync(request.agencyId);

            var userDict = users.ToDictionary(a => a.Id);
            var apartmentDict = apartments.ToDictionary(a => a.Id);

            var contractsToReturn = _mapper.Map<List<ContractToReturnDto>>(contracts);
            foreach (var item in contractsToReturn)
            {
                if (userDict.TryGetValue(item.UserId, out var user))
                    item.User = user;
                if (apartmentDict.TryGetValue(item.ApartmentId, out var apartment))
                    item.Apartment = _mapper.Map<ApartmentToReturnDto>(apartment);

                item.Agency = agency;
            }

            return contractsToReturn;
        }
    }
}