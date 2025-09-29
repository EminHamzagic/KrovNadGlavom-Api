using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Data.DTO.Apartment;
using krov_nad_glavom_api.Data.DTO.Contract;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Contracts
{
    public class GetUserContractsQueryHandler : IRequestHandler<GetUserContractsQuery, PaginatedResponse<ContractToReturnDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserContractsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<ContractToReturnDto>> Handle(GetUserContractsQuery request, CancellationToken cancellationToken)
        {
            var (contractsPage, totalCount, totalPages) = await _unitOfWork.Contracts.GetContractsByUserId(request.userId, request.parameters);

            var agencyIds = contractsPage.Select(c => c.AgencyId).Distinct().ToList();
            var apartmentIds = contractsPage.Select(c => c.ApartmentId).Distinct().ToList();

            var agencies = await _unitOfWork.Agencies.GetAgenciesByIds(agencyIds);
            var apartments = await _unitOfWork.Apartments.GetApartmentsByIds(apartmentIds);
            var user = await _unitOfWork.Users.GetByIdAsync(request.userId);

            var agencyDict = agencies.ToDictionary(a => a.Id);
            var apartmentDict = apartments.ToDictionary(a => a.Id);

            var contractsToReturn = _mapper.Map<List<ContractToReturnDto>>(contractsPage);
            foreach (var item in contractsToReturn)
            {
                if (agencyDict.TryGetValue(item.AgencyId, out var agency))
                    item.Agency = agency;
                if (apartmentDict.TryGetValue(item.ApartmentId, out var apartment))
                    item.Apartment = _mapper.Map<ApartmentToReturnDto>(apartment);

                item.User = user;
            }

            return new PaginatedResponse<ContractToReturnDto>(
                contractsToReturn,
                totalCount,
                request.parameters.PageNumber,
                request.parameters.PageSize,
                totalPages
            );
        }
    }
}