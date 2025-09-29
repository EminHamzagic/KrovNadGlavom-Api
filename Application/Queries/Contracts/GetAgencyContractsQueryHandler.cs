using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Utils;
using krov_nad_glavom_api.Data.DTO.Apartment;
using krov_nad_glavom_api.Data.DTO.Contract;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace krov_nad_glavom_api.Application.Queries.Contracts
{
    public class GetAgencyContractsQueryHandler : IRequestHandler<GetAgencyContractsQuery, PaginatedResponse<ContractToReturnDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAgencyContractsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public async Task<PaginatedResponse<ContractToReturnDto>> Handle(GetAgencyContractsQuery request, CancellationToken cancellationToken)
        {
            var (contractsPage, totalCount, totalPages) = await _unitOfWork.Contracts.GetContractsByAgencyId(request.agencyId, request.parameters);

            var userIds = contractsPage.Select(c => c.UserId).Distinct().ToList();
            var apartmentIds = contractsPage.Select(c => c.ApartmentId).Distinct().ToList();

            var users = await _unitOfWork.Users.GetUsersByIds(userIds);
            var apartments = await _unitOfWork.Apartments.GetApartmentsByIds(apartmentIds);
            var agency = await _unitOfWork.Agencies.GetByIdAsync(request.agencyId);

            var userDict = users.ToDictionary(a => a.Id);
            var apartmentDict = apartments.ToDictionary(a => a.Id);

            var contractsToReturn = _mapper.Map<List<ContractToReturnDto>>(contractsPage);
            foreach (var item in contractsToReturn)
            {
                if (userDict.TryGetValue(item.UserId, out var user))
                    item.User = user;
                if (apartmentDict.TryGetValue(item.ApartmentId, out var apartment))
                    item.Apartment = _mapper.Map<ApartmentToReturnDto>(apartment);

                item.Agency = agency;
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