using krov_nad_glavom_api.Data.DTO.Contract;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Contracts
{
    public class CreateContractCommand : IRequest<Contract>
    {
		public ContractToAddDto ContractToAddDto { get; }
        public CreateContractCommand(ContractToAddDto contractToAddDto)
        {
			ContractToAddDto = contractToAddDto;
		}
	}
}