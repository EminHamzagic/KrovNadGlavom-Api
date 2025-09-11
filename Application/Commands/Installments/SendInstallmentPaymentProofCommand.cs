using krov_nad_glavom_api.Data.DTO.Installment;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Installments
{
    public class SendInstallmentPaymentProofCommand : IRequest<string>
    {
		public InstallmentProofToSendDto InstallmentProofToSendDto { get; }
		public SendInstallmentPaymentProofCommand(InstallmentProofToSendDto installmentProofToSendDto)
        {
			InstallmentProofToSendDto = installmentProofToSendDto;
		}
	}
}