using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Data.DTO.Apartment;
using krov_nad_glavom_api.Data.DTO.Building;
using krov_nad_glavom_api.Data.DTO.Contract;
using MediatR;

namespace krov_nad_glavom_api.Application.Queries.Contracts
{
    public class GetContractByIdQueryHandler : IRequestHandler<GetContractByIdQuery, ContractToReturnDto>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public GetContractByIdQueryHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<ContractToReturnDto> Handle(GetContractByIdQuery request, CancellationToken cancellationToken)
        {
            var contract = await _unitofWork.Contracts.GetByIdAsync(request.Id);
            if (contract == null)
                throw new Exception("Ugovor nije pronađen");

            var installments = await _unitofWork.Installments.GetInstallmentsByContractId(contract.Id);
            var user = await _unitofWork.Users.GetByIdAsync(contract.UserId);
            var agency = await _unitofWork.Agencies.GetByIdAsync(contract.AgencyId);
            var apartment = await _unitofWork.Apartments.GetByIdAsync(contract.ApartmentId);
            var building = await _unitofWork.Buildings.GetByIdAsync(apartment.BuildingId);

            var contractToReturn = _mapper.Map<ContractToReturnDto>(contract);
            contractToReturn.Installments = installments;
            contractToReturn.User = user;
            contractToReturn.Agency = agency;
            contractToReturn.Apartment = _mapper.Map<ApartmentToReturnDto>(apartment);
            contractToReturn.Apartment.Building = _mapper.Map<BuildingToReturnDto>(building);

            return contractToReturn;
        }
    }
}