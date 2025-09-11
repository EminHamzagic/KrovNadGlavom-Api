using krov_nad_glavom_api.Data.DTO.Installment;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Users
{
    public class SetUserProfileImageCommand : IRequest<string>
    {
		public InstallmentProofToSendDto InstallmentProofToSendDto { get; }
        public SetUserProfileImageCommand(InstallmentProofToSendDto installmentProofToSendDto)
        {
			InstallmentProofToSendDto = installmentProofToSendDto;
		}
	}
}