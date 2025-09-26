using AutoMapper;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Contracts
{
    public class CreateContractCommandHandler : IRequestHandler<CreateContractCommand, Contract>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public CreateContractCommandHandler(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<Contract> Handle(CreateContractCommand request, CancellationToken cancellationToken)
        {
            var apartment = await _unitofWork.Apartments.GetByIdAsync(request.ContractToAddDto.ApartmentId);
            if (apartment == null)
                throw new Exception("Stan nije pronađen");

            if(!apartment.IsAvailable)
                throw new Exception("Stan je već kupljen");

            var contract = _mapper.Map<Contract>(request.ContractToAddDto);
            contract.Id = Guid.NewGuid().ToString();

            _unitofWork.Contracts.AddAsync(contract);

            var firstInstallment = new Installment
            {
                Id = Guid.NewGuid().ToString(),
                ContractId = contract.Id,
                SequenceNumber = 1,
                Amount = contract.InstallmentAmount,
                DueDate = DateTime.Now.AddDays(30),
                IsConfirmed = false,
                CreatedAt = DateTime.Now
            };
            _unitofWork.Installments.AddAsync(firstInstallment);

            apartment.IsAvailable = false;
            await _unitofWork.Save();

            return contract;
        }
    }
}