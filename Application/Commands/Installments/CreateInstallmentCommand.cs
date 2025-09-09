using krov_nad_glavom_api.Data.DTO.Installment;
using krov_nad_glavom_api.Domain.Entities;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Installments
{
    public class CreateInstallmentCommand : IRequest<Installment>
    {
		public InstallmentToAddDto InstallmentToAddDto { get; }
        public CreateInstallmentCommand(InstallmentToAddDto installmentToAddDto)
        {
			InstallmentToAddDto = installmentToAddDto;
		}
	}
}