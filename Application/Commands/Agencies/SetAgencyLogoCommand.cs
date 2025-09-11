using krov_nad_glavom_api.Data.DTO.Installment;
using MediatR;

namespace krov_nad_glavom_api.Application.Commands.Agencies
{
    public class SetAgencyLogoCommand : IRequest<string>
    {
        public InstallmentProofToSendDto Dto { get; }
        public SetAgencyLogoCommand(InstallmentProofToSendDto dto)
        {
			Dto = dto;
		}
    }
}