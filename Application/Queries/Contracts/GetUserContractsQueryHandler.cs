using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Data.DTO.Apartment;
using krov_nad_glavom_api.Data.DTO.Contract;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace krov_nad_glavom_api.Application.Queries.Contracts
{
    public class GetUserContractsQueryHandler : IRequestHandler<GetUserContractsQuery, PaginatedResponse<ContractToReturnDto>>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public GetUserContractsQueryHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<ContractToReturnDto>> Handle(GetUserContractsQuery request, CancellationToken cancellationToken)
        {
            var contractsQuery = _unitofWork.Contracts.GetContractsByUserId(request.userId, request.parameters.Status);

            var totalCount = contractsQuery.Count();
            request.parameters.checkOverflow(totalCount);

            var contractsPage = await contractsQuery
                .Skip((request.parameters.PageNumber - 1) * request.parameters.PageSize)
                .Take(request.parameters.PageSize)
                .ToListAsync(cancellationToken);

            var agencyIds = contractsPage.Select(c => c.AgencyId).Distinct().ToList();
            var apartmentIds = contractsPage.Select(c => c.ApartmentId).Distinct().ToList();

            var agencies = await _unitofWork.Agencies.GetAgenciesByIds(agencyIds);
            var apartments = await _unitofWork.Apartments.GetApartmentsByIds(apartmentIds);
            var user = await _unitofWork.Users.GetByIdAsync(request.userId);

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

            var totalPages = (int)Math.Ceiling((double)totalCount / request.parameters.PageSize);

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