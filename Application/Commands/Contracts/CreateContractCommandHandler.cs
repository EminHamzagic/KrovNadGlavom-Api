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
            var contract = _mapper.Map<Contract>(request.ContractToAddDto);
            contract.Id = Guid.NewGuid().ToString();

            _unitofWork.Contracts.AddAsync(contract);
            await _unitofWork.Save();

            return contract;
        }
    }
}